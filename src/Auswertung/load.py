import os
from expression import pipe
from expression.collections import seq
from expression.collections import Seq
import pandas as pd
import yaml
import matplotlib.pyplot as plt
import numpy as np


def is_directory(path):
    return os.path.isdir(path)


class QueryData:
    def __init__(self, name, path):
        self.name = name
        self.path = path


class Scenario:
    def __init__(self, name, table):
        self.name = name
        self.table = table


def query_data(folder_item):
    return list(
        pipe(
            seq.of_iterable(os.scandir(folder_item.path)),
            seq.filter(lambda e: e.name == "query_data.csv"),
            seq.map(lambda e: QueryData(folder_item.name, e.path))
        )
    )


def add_scenario_column(scenario):
    scenario.table.insert(
        0,
        "scenario",
        list(Seq(range(0, scenario.table.shape[0], 1)).map(lambda x: scenario.name)),
        True)
    return scenario


def load_folders():
    return pipe(
        seq.of_iterable(os.scandir(path='./data/results')),
        seq.filter(is_directory)
    )


def rename_dependencies(table):
    return table.rename(columns={
        "table_percentile_duration_25": "dependency_percentile_duration_25",
        "table_percentile_duration_50": "dependency_percentile_duration_50",
        "table_percentile_duration_75": "dependency_percentile_duration_75",
        "table_percentile_duration_90": "dependency_percentile_duration_90",
        "table_percentile_duration_95": "dependency_percentile_duration_95",
        "table_percentile_duration_99": "dependency_percentile_duration_99",
        "table_min_duration": "dependency_min_duration",
        "table_max_duration": "dependency_max_duration",
        "table_average_duration": "dependency_average_duration",
        "table_request_count": "dependency_request_count",
        "sql_percentile_duration_25": "dependency_percentile_duration_25",
        "sql_percentile_duration_50": "dependency_percentile_duration_50",
        "sql_percentile_duration_75": "dependency_percentile_duration_75",
        "sql_percentile_duration_90": "dependency_percentile_duration_90",
        "sql_percentile_duration_95": "dependency_percentile_duration_95",
        "sql_percentile_duration_99": "dependency_percentile_duration_99",
        "sql_min_duration": "dependency_min_duration",
        "sql_max_duration": "dependency_max_duration",
        "sql_average_duration": "dependency_average_duration",
        "sql_request_count": "dependency_request_count",
    })


def read_and_rename(path):
    rename_dependencies(pd.read_csv(path))


def load_as_scenarios():
    dfs = list(pipe(
        load_folders(),
        seq.map(query_data),
        seq.fold(lambda x, y: x + y, list()),
        seq.map(lambda x: (x.name, read_and_rename(x.path)))
    ))
    return dfs


def load_all_as_pd():
    dfs = list(pipe(
        load_folders(),
        seq.map(query_data),
        seq.fold(lambda x, y: x + y, list()),
        seq.map(lambda x: Scenario(x.name, pd.read_csv(x.path))),
        seq.map(add_scenario_column),
        seq.map(lambda scenario: scenario.table),
        seq.map(rename_dependencies)
    ))
    return pd.concat(dfs)


# x = load_all_as_pd()
# print()
