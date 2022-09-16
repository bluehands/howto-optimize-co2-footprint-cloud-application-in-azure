import os
import re

from expression import pipe
from expression.collections import seq
from expression.collections import Seq
import pandas as pd
import yaml
import matplotlib.pyplot as plt
import numpy as np
import plotly.graph_objects as go

from utils import rename_ids, get_hosting_environment_type, get_scenario_load_level, format_load_level, \
    load_level_to_numeric, get_scenario_communication

from utils import rename_ids

messaging = pd.concat([
    pd.read_csv('./data/exports/messaging_duration.csv'),
    pd.read_csv('./data/exports/messaging_duration_1.csv')
])

messaging.rename(columns={"_ResourceId": "name"}, inplace=True)


messaging['name'] = messaging['name'].apply(rename_ids)


messaging["hosting_type"] = messaging["name"].apply(get_hosting_environment_type)


messaging["scenario_load_level"] = messaging["name"].apply(lambda x: get_scenario_load_level(x, "messaging"))

messaging["scenario_communication"] = messaging["name"].apply(lambda x: get_scenario_communication(x, "messaging"))

messaging["name_index"] = "  " + messaging["hosting_type"] + " | " \
                          + messaging["scenario_communication"]

messaging["lv"] = messaging['scenario_load_level'].apply(format_load_level)
messaging["lv_num"] = messaging['scenario_load_level'].apply(load_level_to_numeric)

messaging["rps"] = messaging["rcount"] / (2*60*60)

res = messaging[['lv_num', "name_index", "rps", "rcount", "percentile_Sum_50", "percentile_Sum_75", "percentile_Sum_95", "percentile_Sum_99"]]



res.sort_values(by=['lv_num', 'name_index'], ascending=[False, True], inplace=True)

groups = res.groupby("lv_num")

for name, group in groups:
    print(group.to_latex())


print("")