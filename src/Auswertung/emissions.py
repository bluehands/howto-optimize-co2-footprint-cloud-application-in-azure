import math
import re

import numpy as np
import pandas as pd

from load import load_all_as_pd
from datetime import datetime, timedelta
from expression import pipe
from expression.collections import seq
from expression.collections import Seq
import matplotlib.pyplot as plt

from utils import get_hosting_environment_type, get_scenario_type, get_scenario_communication, get_scenario_load_level, \
    get_os

MIN_WATTS_AVG = 0.74
MAX_WATTS_AVG = 3.54

MEMORY_AVG = 80.47  # gigaBytes / physical chip

SSDCOEFFICIENT = 1.2  # watt hours / terabyte hour
HDDCOEFFICIENT = 0.65  # watt hours / terabyte hour

PUE_AVG = 1.185

SERVER_EXPECTED_LIFESPAN = 35040

NETWORKING_COEFFICIENT = 0.001  # // kWh / Gb
MEMORY_COEFFICIENT = 0.000392  # // kWh / Gb


def get_env_data(instance_row):
    return {
        "App Service": {
            "min_w": MIN_WATTS_AVG,
            "max_w": MAX_WATTS_AVG,
            "cpu_util_avg": instance_row["average_processor_time_normalized"],
            "cpu": 2.0,
            "ram_bytes": 8000000.0,
            "duration": "02:00:00",
        },
        "VM": {
            "min_w": 0.856403566919192,
            "max_w": 3.9346685380591633,
            "cpu_util_avg": instance_row["average_processor_time_normalized"],
            "cpu": 2.0,
            "ram_bytes": 8000000.0,
            "duration": "02:00:00",
        },
        "Function": {
            "min_w": MIN_WATTS_AVG,
            "max_w": MAX_WATTS_AVG,
            "cpu_util_avg": instance_row["average_processor_time_normalized"],
            "cpu": instance_row["cpu_count"],
            "ram_bytes": instance_row["average_ram_used_bytes"],
            "duration": instance_row["instance_duration"],
        },
        "Container Instance": {
            "min_w": MIN_WATTS_AVG,
            "max_w": MAX_WATTS_AVG,
            "cpu_util_avg": instance_row["average_processor_time_normalized"],
            "cpu": 2.0,
            "ram_bytes": 8000000.0,
            "duration": "02:00:00",
        }
    }


def parse_timedelta_from(time_string):
    time_string = re.sub(r'\.\d+', "", time_string)
    t = datetime.strptime(time_string, "%H:%M:%S")
    return timedelta(hours=t.hour, minutes=t.minute, seconds=t.second)


def calculate_compute_energy_by(env_data):
    avg_watt = env_data["min_w"] + (env_data["cpu_util_avg"] / 100.0) * (env_data["max_w"] - env_data["min_w"])
    cpu_h = env_data["cpu"] * to_hours(env_data["duration"])
    return dict(watt_2h=avg_watt * cpu_h, cpu_h=cpu_h, cpu_count=env_data["cpu"], avg_cpu=env_data["cpu_util_avg"])


def to_hours(duration):
    return (parse_timedelta_from(duration).total_seconds() / 60.0) / 60.0


def calculate_compute_energy(data):

    groups = data.groupby("scenario")

    res = list()
    for name, group in groups:
        hosting_type = get_hosting_environment_type(name)
        scenario_type = get_scenario_type(name)
        scenario_communication = get_scenario_communication(name, scenario_type)
        scenario_load_level = get_scenario_load_level(name, scenario_type)
        scenario_data = list()
        os = get_os(name)
        for index, instance_row in group.iterrows():
            env_data = get_env_data(instance_row)[hosting_type]
            scenario_data.append(calculate_compute_energy_by(env_data))
        res.append(
            dict(
                name=name,
                hosting_type=hosting_type,
                scenario_type=scenario_type,
                scenario_communication=scenario_communication,
                scenario_load_level=scenario_load_level,
                scenario_data=scenario_data,
                os=os,
            )
        )
    return res


def energy_and_v_cpu_h(data, duration):
    return pd.DataFrame(
        list(
            pipe(
                seq.of_iterable(calculate_compute_energy(data)),
                seq.map(
                    lambda scenario: dict(
                        name=scenario["name"],
                        hosting_type=scenario["hosting_type"],
                        scenario_type=scenario["scenario_type"],
                        scenario_communication=scenario["scenario_communication"],
                        scenario_load_level=scenario["scenario_load_level"],
                        watt_h_compute=pipe(
                            seq.of_iterable(scenario["scenario_data"]),
                            seq.fold(lambda acc, scenario_data: acc + scenario_data["watt_2h"], 0)
                        ) / duration,
                        v_cpu_h=pipe(
                            seq.of_iterable(scenario["scenario_data"]),
                            seq.fold(lambda acc, scenario_data: acc + scenario_data["cpu_h"], 0)
                        ) / duration,
                        os=scenario["os"],
                    )
                )
            )
        )
    )


def scope_3_per_core_h_by(lifetime_years):
    azure_instances = pd.read_csv("./data/ccf/azure-instances.csv")
    azure_instances_series = azure_instances.groupby("Series").agg({"Platform vCPUs (highest vCPU possible)": "max"})
    scope_3_data = pd.read_csv("./data/ccf/coefficients-azure-embodied.csv")
    scope_3_types = scope_3_data.groupby("family")
    scope_3_types = scope_3_types.agg({"total": "max"})
    general_purpose_vms = [
        # "Av2 Standard",
        # "Bs-series",
        #  "D1-5 v2",
        #  "D1s-5s v2",
        "D2 – D64 v4",
        "D2-64 v3",
        "D2a – D96a v4",
        "D2as – D96as v4",
        "D2d – D64d v4",
        "D2ds – D64ds v4"
        "D2s – D64s v4"
        "D2s-64s v3"
        #  "DCsv2-series"
    ]
    general_purpose_scope_3 = scope_3_types[scope_3_types.index.isin(general_purpose_vms)]

    kg_per_core = pd.DataFrame(
        pipe(
            seq.of_iterable(general_purpose_scope_3.itertuples()),
            seq.map(lambda x: {"name": x[0], "total": x[1] / azure_instances_series.T[x[0]][0]})
        )
    )

    year_h = (365 * lifetime_years) * 24
    emissions_per_core = kg_per_core.agg({"total": ["mean", "sem"]})
    kg_emissions_per_core_h = emissions_per_core.apply(
        func=lambda x: x / year_h).T
    kg_emissions_per_core_h["sem_percent"] = kg_emissions_per_core_h["sem"] / kg_emissions_per_core_h["mean"] * 100
    return kg_emissions_per_core_h


def total_emissions(grid_metric_gramms, hardware_lifetime_years, data, duration):
    kg_scope_3_emissions_per_core_h = scope_3_per_core_h_by(hardware_lifetime_years)
    table = energy_and_v_cpu_h(data, duration)
    table["co2_compute"] = (table[
                                "watt_h_compute"] / 1000) * PUE_AVG * grid_metric_gramms
    table["co2_scope_3"] = table["v_cpu_h"] * (kg_scope_3_emissions_per_core_h["mean"][0] * 1000)
    table["total_co2"] = table["co2_compute"] + table["co2_scope_3"]
    table["co2_scope_3_percent"] = table["co2_scope_3"] / table["total_co2"]
    table["co2_compute_percent"] = table["co2_compute"] / table["total_co2"]
    table["co2_scope_3_err"] = table["v_cpu_h"] * (kg_scope_3_emissions_per_core_h["sem"][0] * 1000)
    return table
