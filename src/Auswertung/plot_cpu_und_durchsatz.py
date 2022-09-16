import math

import numpy
import pandas as pd

from emissions import get_env_data, calculate_compute_energy_by, to_hours, energy_and_v_cpu_h
from load import load_all_as_pd
from utils import get_hosting_environment_type, get_scenario_type, get_scenario_communication, get_scenario_load_level, \
    get_os
from plotly.subplots import make_subplots
import plotly.graph_objects as go

from expression import pipe
from expression.collections import seq
from expression.collections import Seq


def wh(x, max):
    if x[0] == 0:
        return 0
    else:
        w = x[0] / max
        res = (w) * x[1]
        return res


def calc_cpu(duration=2.0):
    data = load_all_as_pd()

    groups = data.groupby("scenario")

    res = list()
    for name, group in groups:
        hosting_type = get_hosting_environment_type(name)
        scenario_type = get_scenario_type(name)
        scenario_communication = get_scenario_communication(name, scenario_type)
        scenario_load_level = get_scenario_load_level(name, scenario_type)
        os = get_os(name)

        mean_err = group.agg({"average_processor_time_normalized": ["median", "mean", "sem"]}).fillna(0).T

        rps = group.agg({"incoming_request_count": "sum"})[0] / (2 * 60 * 60)

        sum_h = pipe(
            seq.of_iterable(group["instance_duration"]),
            seq.map(to_hours),
            seq.fold(lambda acc, x: acc + x, 0)
        )

        tmp = energy_and_v_cpu_h(data, duration).groupby("name").get_group(name)

        res.append(
            dict(
                name=name,
                hosting_type=hosting_type,
                scenario_type=scenario_type,
                scenario_communication=scenario_communication,
                scenario_load_level=scenario_load_level,
                rps=rps,
                cpu_med=mean_err["median"][0],
                cpu_avg=mean_err["mean"][0],
                cpu_avg_err=mean_err["sem"][0],
                os=os,
                v_cpu_h=tmp["v_cpu_h"].values[0],
                watt_h=tmp["watt_h_compute"].values[0]
            )
        )
    return res


result = pd.DataFrame(calc_cpu())

result.drop(result[result["name"] == "reports-2-appservice"].index, inplace=True)
result.drop(result[result["name"] == "reports-25-appservice"].index, inplace=True)
result.drop(result[result["name"] == "reports-250-appservice"].index, inplace=True)

result["bname"] = result["hosting_type"]
result.sort_values(by=["scenario_communication", "bname"], inplace=True)

fig = make_subplots(rows=4, cols=3, vertical_spacing=0.16,
                    # subplot_titles=('', 'Anfragen / Sekunde', "", "", "CPU-Auslastung", "")
                    )

groups = result.groupby("scenario_load_level")

low = groups.get_group("low")
mid = groups.get_group("mid")
high = groups.get_group("high")
fig.add_trace(
    go.Bar(
        name="Anfragen / Sekunde",
        x=[low["scenario_communication"], low["bname"]], y=low["rps"],
        showlegend=False,
        marker=dict(
            color='rgb(0,191,255)')
    ),
    row=1, col=1
)
fig.add_trace(
    go.Bar(
        name="Anfragen / Sekunde",
        x=[mid["scenario_communication"], mid["bname"]], y=mid["rps"],
        showlegend=False,
        marker=dict(
            color='rgb(30,144,255)')
    ),
    row=1, col=2
)
fig.add_trace(
    go.Bar(
        name="Anfragen / Sekunde",
        x=[high["scenario_communication"], high["bname"]], y=high["rps"],
        showlegend=False,
        marker=dict(
            color='rgb(65,105,225)')
    ),
    row=1, col=3
)
fig.add_trace(
    go.Bar(
        name="Durschnittliche CPU-Auslastung",
        x=[low["scenario_communication"], low["bname"]], y=low["cpu_avg"],
        showlegend=False,
        marker=dict(
            color='rgb(0,191,255)')
    ),
    row=2, col=1
)
fig.add_trace(
    go.Bar(
        name="Durschnittliche CPU-Auslastung",
        x=[mid["scenario_communication"], mid["bname"]], y=mid["cpu_avg"],
        showlegend=False,
        marker=dict(
            color='rgb(30,144,255)')
    ),
    row=2, col=2
)
fig.add_trace(
    go.Bar(
        name="Durschnittliche CPU-Auslastung",
        x=[high["scenario_communication"], high["bname"]],
        y=high["cpu_avg"],
        showlegend=False,
        marker=dict(
            color='rgb(65,105,225)')
    ),
    row=2, col=3
)
# ---------------------------------
fig.add_trace(
    go.Bar(
        name="vCPU-Stunden",
        x=[low["scenario_communication"], low["bname"]],
        y=low["v_cpu_h"],
        showlegend=False,
        marker=dict(
            color='rgb(0,191,255)')
    ),
    row=3, col=1
)
fig.add_trace(
    go.Bar(
        name="vCPU-Stunden",
        x=[mid["scenario_communication"], mid["bname"]],
        y=mid["v_cpu_h"],
        showlegend=False,
        marker=dict(
            color='rgb(30,144,255)')
    ),
    row=3, col=2
)
fig.add_trace(
    go.Bar(
        name="vCPU-Stunden",
        x=[high["scenario_communication"], high["bname"]],
        y=high["v_cpu_h"],
        showlegend=False,
        marker=dict(
            color='rgb(65,105,225)')
    ),
    row=3, col=3
)

# ---------------------------------
fig.add_trace(
    go.Bar(
        name="Wattstunden",
        x=[low["scenario_communication"], low["bname"]],
        y=low["watt_h"],
        showlegend=False,
        marker=dict(
            color='rgb(0,191,255)')
    ),
    row=4, col=1
)
fig.add_trace(
    go.Bar(
        name="Wattstunden",
        x=[mid["scenario_communication"], mid["bname"]],
        y=mid["watt_h"],
        showlegend=False,
        marker=dict(
            color='rgb(30,144,255)')
    ),
    row=4, col=2
)
fig.add_trace(
    go.Bar(
        name="Wattstunden",
        x=[high["scenario_communication"], high["bname"]],
        y=high["watt_h"],
        showlegend=False,
        marker=dict(
            color='rgb(65,105,225)')
    ),
    row=4, col=3
)

fig.update_layout(
    autosize=False,
    width=1600,
    height=1600 * 1.4142135,  # * 1.4142135,
    title=dict(
        text="Messwerte der Szenarien",
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

fig.update_layout(barmode='group')

# fig.update_layout(
#     yaxis1=dict(
#         range=[0, 260],
#     ),
#     yaxis2=dict(
#         range=[0, 260],
#     ),
#     yaxis3=dict(
#         range=[0, 260],
#     ),
#     yaxis4=dict(
#         range=[0, 100],
#     ),
#     yaxis5=dict(
#         range=[0, 100],
#     ),
#     yaxis6=dict(
#         range=[0, 100],
#     ),
# )

fig['layout']['yaxis']['title'] = 'Anfragen / Sekunde'
# fig['layout']['xaxis2']['title']='Label y-axis 2'
# fig['layout']['xaxis3']['title']='Label y-axis 2'
fig['layout']['yaxis4']['title'] = '% vCPU-Auslastung'
fig['layout']['yaxis7']['title'] = 'vCPU-Stunden'
fig['layout']['yaxis10']['title'] = 'Wattstunden'
# fig['layout']['xaxis5']['title']='Label y-axis 2'
# fig['layout']['xaxis6']['title']='Label y-axis 2'

fig['layout']['xaxis']['title'] = 'Niedrige Last'
fig['layout']['xaxis2']['title'] = 'Mittlere Last'
fig['layout']['xaxis3']['title'] = 'Hohe Last'
fig['layout']['xaxis4']['title'] = 'Niedrige Last'
fig['layout']['xaxis5']['title'] = 'Mittlere Last'
fig['layout']['xaxis6']['title'] = 'Hohe Last'

fig['layout']['xaxis7']['title'] = 'Niedrige Last'
fig['layout']['xaxis8']['title'] = 'Mittlere Last'
fig['layout']['xaxis9']['title'] = 'Hohe Last'
fig['layout']['xaxis10']['title'] = 'Niedrige Last'
fig['layout']['xaxis11']['title'] = 'Mittlere Last'
fig['layout']['xaxis12']['title'] = 'Hohe Last'

fig.show()
fig.write_image("C:/Development/DHBW/dhbw-bachelorarbeit/assets/images/requests/cpu-rps.png")
print()
