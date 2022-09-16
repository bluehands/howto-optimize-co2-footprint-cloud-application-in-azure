from emissions import get_env_data, calculate_compute_energy_by, total_emissions
from load import load_all_as_pd, rename_dependencies
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


import plotly.graph_objects as go
import plotly.express as px

from load import load_all_as_pd
from utils import format_load_level, load_level_to_numeric

from utils import get_hosting_environment_type, get_scenario_type, get_scenario_communication, get_scenario_load_level, \
    get_os, format_load_level, load_level_to_numeric

data = load_all_as_pd()

min20 = rename_dependencies(
    pd.read_csv("./data/functions/query_data.csv")
)

min20.insert(
    0,
    "scenario",
    "function-min20",
    True)
min8 = rename_dependencies(
    pd.read_csv("./data/functions/query_data_1.csv")
)
min8.insert(
    0,
    "scenario",
    "function-min8",
    True)
min1 = rename_dependencies(
    pd.read_csv("./data/functions/query_data_3.csv")
)
min1.insert(
    0,
    "scenario",
    "function-min1",
    True)

functions = pd.concat([min20, min8, min1])

eu_grid = 229
de_grid = 314 # 2020
fr_grid = 60
se_grid = 8





def formatlv(old):
    if old == "low":
        return "2,5/sec"
    if old == "mid":
        return "25/sec"
    if old == "high":
        return "250/sec"
    if old == "min1":
        return "1/min"
    if old == "min8":
        return "8/min"
    if old == "min20":
        return "20/min"

def formatlvnum(old):
    if old == "low":
        return 3
    if old == "mid":
        return 4
    if old == "high":
        return 5
    if old == "min1":
        return 0
    if old == "min8":
        return 1
    if old == "min20":
        return 2

def plot(table, title, w, h, ymax):
    fig = go.Figure()
    table["name_index"] = "  " + table["hosting_type"] + " | " + table["scenario_communication"]
    table["lv"] = table['scenario_load_level'].apply(formatlv)
    table["lv_num"] = table['scenario_load_level'].apply(formatlvnum)
    table.sort_values(by=['name_index', "lv_num"], inplace=True)

    gindex = [table["lv"], table["name_index"]]

    fig.add_trace(
        go.Bar(
            name="Scope 3-Emissionen",
            x=gindex,
            y=table["co2_scope_3"],
            offsetgroup=0,
            # marker=dict(color=['rgb(105,105,105)'] * len(table["co2_compute"])),
            marker=dict(color=["rgb(55, 83, 109)"] * len(table["co2_compute"])),
        ),

    )
    fig.add_trace(
        go.Bar(
            name="Scope 2-Emissionen",
            x=gindex,
            y=table["co2_compute"],
            base=table["co2_scope_3"],
            offsetgroup=0,
            error_y=dict(type='data', array=table["co2_scope_3_err"]),

            # marker=dict(color=["rgb(176,196,222)"] * len(table["co2_compute"])),
            marker=dict(color=['rgb(26, 118, 255)'] * len(table["co2_compute"])),

        ),
    )

    fig.update_layout(barmode='group')
    # size = 1400
    fig.update_xaxes(tickangle=90)
    fig.update_layout(
        autosize=False,
        width=w,
        height=h,  # * 1.4142135,
        title=dict(
            text=title,
            y=0.97,  # new
            x=0.5,
            xanchor='center',
            yanchor='top',
            font=dict(
                size=20,
            ),
        ),
        yaxis_title="Gramm CO<sub>2</sub>-äquivalent / Stunde (gCO<sub>2</sub>e/h)",
        font=dict(
            size=20,
        ),
        yaxis_range=[0, ymax]
        # paper_bgcolor='rgba(0,0,0,0)',
        # plot_bgcolor='rgba(0,0,0,0)'

    )
    fig.show()

def calc(grid):
    tmp = total_emissions(grid, 4, functions, 1.0)
    tmp_ba = total_emissions(grid, 4, load_all_as_pd(), 2.0)

    final_result = pd.concat([tmp, tmp_ba])
    groups = final_result.groupby("hosting_type")
    function_results = groups.get_group("Function")

    function_results = function_results.groupby("scenario_communication").get_group("Service Bus")
    plot(function_results, "CO<sub>2</sub>-äquivalente Emissionen von serverlosen Azure Functions", 1400, 1400, 35)
        #fig.write_image(out)

calc(eu_grid)
calc(se_grid)
print("")
