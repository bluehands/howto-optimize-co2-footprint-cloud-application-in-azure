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


def load_request_reply_dependencies():
    temp = pd.read_csv('./data/exports/dependencies.csv')
    temp.rename(columns={"_ResourceId": "name"}, inplace=True)
    temp['name'] = temp['name'].apply(rename_ids)
    return temp


def load_messaging_dependencies():
    tmp = pd.concat([
        pd.read_csv('./data/exports/dependencies_messaging.csv'),
        pd.read_csv('./data/exports/dependencies_messaging_1.csv')
    ])
    tmp.rename(columns={"_ResourceId": "name"}, inplace=True)
    tmp['name'] = tmp['name'].apply(rename_ids)
    return tmp


def build_dependency_boxplot(table):
    stats = []
    for index, row in table.iterrows():
        iqr = row['percentile_DurationMs_75'] - row['percentile_DurationMs_25']
        whislo = row['percentile_DurationMs_25'] - 1.5 * iqr
        whishi = row['percentile_DurationMs_75'] + 1.5 * iqr
        plot = {
            "label": row['name_index'],  # not required
            "mean": row['avg'],  # not required
            "med": row['percentile_DurationMs_50'],
            "q1": row['percentile_DurationMs_25'],
            "q3": row['percentile_DurationMs_75'],
            # "cilo": 5.3 # not required
            # "cihi": 5.7 # not required
            "whislo": row['min'],  # required
            "whishi": row['percentile_DurationMs_90'],  # required
            "fliers": []  # required if showfliers=True
        }
        stats.append(plot)
    return stats


messaging = load_messaging_dependencies()
request_reply = load_request_reply_dependencies()

messaging["hosting_type"] = messaging["name"].apply(get_hosting_environment_type)
request_reply["hosting_type"] = request_reply["name"].apply(get_hosting_environment_type)

messaging["scenario_load_level"] = messaging["name"].apply(lambda x: get_scenario_load_level(x, "messaging"))
request_reply["scenario_load_level"] = request_reply["name"].apply(lambda x: get_scenario_load_level(x, "messaging"))

messaging["scenario_communication"] = messaging["name"].apply(lambda x: get_scenario_communication(x, "messaging"))
request_reply["scenario_communication"] = request_reply["name"].apply(
    lambda x: get_scenario_communication(x, "request-reply"))

messaging["lv"] = messaging['scenario_load_level'].apply(format_load_level)
messaging["lv_num"] = messaging['scenario_load_level'].apply(load_level_to_numeric)
request_reply["lv"] = request_reply['scenario_load_level'].apply(format_load_level)
request_reply["lv_num"] = request_reply['scenario_load_level'].apply(load_level_to_numeric)

messaging["name_index"] = "  " + messaging["hosting_type"] + " | " \
                          + messaging["scenario_communication"]
request_reply["name_index"] = "  " + request_reply["hosting_type"] \
                              + " | " + request_reply["scenario_communication"]

messaging.sort_values(by=['lv_num', 'name_index'], ascending=[False, True], inplace=True)
request_reply.sort_values(by=['lv_num', 'name_index'], ascending=[False, True], inplace=True)


def plot_dependencies(df, title, w, h, out):
    fig = go.Figure()
    groups = df.groupby("lv_num")
    for name, g in groups:
        fig.add_trace(go.Box(
            name=g["lv"].iloc[0],
            x=g["name_index"],
            q1=g['percentile_DurationMs_25'],
            median=g['percentile_DurationMs_50'],
            q3=g['percentile_DurationMs_75'],
            lowerfence=g["percentile_DurationMs_10"],
            upperfence=g['percentile_DurationMs_90'],
            mean=g['avg'],
            #    sd=[0.2, 0.4, 0.6],
            #    notchspan=[0.2, 0.4, 0.6]
        ))
    fig.update_layout(boxmode='group')
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
                size=28,
            ),
        ),
        yaxis_title="Dauer in Millisekunden",
        xaxis_title_standoff=12,
        font=dict(
            size=20,
        )
    )
    fig.update_layout(margin_pad=10)
    fig.update_xaxes(tickangle=30)
    #fig.show()
    fig.write_image(out)


plot_dependencies(messaging, "Dauer der Abhängigkeiten bei temporal entkoppelter Kommunikation", 1600, 1600,
                  "C:/Development/DHBW/dhbw-bachelorarbeit/assets/images/dependencies/table.png")
plot_dependencies(request_reply, "Dauer der Abhängigkeiten bei temporal gekoppelter Kommunikation", 1600, 1600,
                  "C:/Development/DHBW/dhbw-bachelorarbeit/assets/images/dependencies/sql.png")

# def render_boxplot(data, title, outfile, size):
#     px = 1 / plt.rcParams['figure.dpi']
#     fig, axes = plt.subplots(nrows=1, ncols=1, figsize=(size * px, size * px))
#     axes.bxp(data)
#     axes.set_title(title, fontsize=20)
#     axes.set_ylabel('Dauer in Millisekunden')
#     axes.set_xlabel('Szenario')
#     fig.autofmt_xdate(rotation=45)
#
#     plt.savefig(outfile)
#     plt.show()
#
#
# render_boxplot(build_dependency_boxplot(messaging), 'Dauer der Abhängigkeiten (Azure Cosmos DB Tables)',
#                "C:/Development/DHBW/dhbw-bachelorarbeit/assets/images/dependencies/sql.png", 1500)
#
# render_boxplot(build_dependency_boxplot(request_reply), 'Dauer der Abhängigkeiten (Azure SQL Database)',
#                "C:/Development/DHBW/dhbw-bachelorarbeit/assets/images/dependencies/table.png", 1500)
