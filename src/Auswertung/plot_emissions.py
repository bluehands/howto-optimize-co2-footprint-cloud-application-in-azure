import pandas as pd

from emissions import total_emissions
import plotly.graph_objects as go
import plotly.express as px

from load import load_all_as_pd
from utils import format_load_level, load_level_to_numeric


def plot_by_hosting_type_and_comm(table, title):
    fig = go.Figure()
    groupby = table.groupby(["hosting_type", "scenario_communication"])
    for keys, group in groupby:
        by_level = group.groupby("scenario_load_level")
        name, tech = keys
        low = by_level.get_group("low")
        mid = by_level.get_group("mid")
        high = by_level.get_group("high")

        arr = [low["total_co2"].values[0], mid["total_co2"].values[0], high["total_co2"].values[0]]

        fig.add_trace(go.Bar(
            name=name + ", " + tech,
            x=['Niedrigige Auslastung', 'Mittelere Auslastung', 'Hohe Auslastung'], y=arr,
            error_y=dict(type='data', array=[low["co2_scope_3_err"].values[0], mid["co2_scope_3_err"].values[0], high[
                "co2_scope_3_err"].values[0]])
        ))

    fig.update_layout(barmode='group')
    # size = 1000
    fig.update_layout(
        # width=size,
        # height=size * 1.4142135,
        title=dict(
            text=title,
            y=0.97,  # new
            x=0.5,
            xanchor='center',
            yanchor='top',
            font=dict(
                size=30,
            ),
        ),
        yaxis_title="Gramm CO<sub>2</sub>-Äquivalent / Stunde (gCO<sub>2</sub>e/h)",
        font=dict(
            size=20,
        ),
        # paper_bgcolor='rgba(0,0,0,0)',
        # plot_bgcolor='rgba(0,0,0,0)'

    )
    fig.show()


def plot_by_hosting_type_and_com_stacked(table, title, w, h, out, ymax):
    fig = go.Figure()
    table["name_index"] = "  " + table["hosting_type"] + " | " + table["scenario_communication"] + " | " + table["os"]
    table["lv"] = table['scenario_load_level'].apply(format_load_level)
    table["lv_num"] = table['scenario_load_level'].apply(load_level_to_numeric)
    table.sort_values(by=['name_index', 'lv_num'], inplace=True)

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
    # fig.show()
    fig.write_image(out)


def plot_emissions(factors, years, t_suffix, f_suffix, y_max_g, y_max_e):
    result = total_emissions(factors, years, load_all_as_pd(), 2.0)
    types = result.groupby("scenario_type")
    messaging = types.get_group("messaging")
    request_reply = types.get_group("request-reply")
    scale = 1
    plot_by_hosting_type_and_com_stacked(request_reply,
                                         "CO<sub>2</sub>-äquivalente Emissionen bei temporal gekoppelter Kommunikation" + t_suffix,
                                         1400 * scale,
                                         1400 * scale,
                                         "C:/Development/DHBW/dhbw-bachelorarbeit/assets/images/emissionen/emissionen-temporal-gekoppelt" + f_suffix,
                                         y_max_g)
    plot_by_hosting_type_and_com_stacked(messaging,
                                         "CO<sub>2</sub>-äquivalente Emissionen bei temporal entkoppelter Kommunikation" + t_suffix,
                                         1400 * scale,
                                         1400 * scale,
                                         "C:/Development/DHBW/dhbw-bachelorarbeit/assets/images/emissionen/emissionen-temporal-entkoppelt" + f_suffix,
                                         y_max_e)


def plot_group(factors, years, t_suffix, f_suffix, y_max_g, y_max_e, group):
    result = total_emissions(factors, years, 2.0)
    types = result.groupby("scenario_type")
    messaging = types.get_group("messaging")
    request_reply = types.get_group("request-reply")
    messaging = messaging.groupby("scenario_load_level").get_group(group)
    request_reply = request_reply.groupby("scenario_load_level").get_group(group)
    plot_by_hosting_type_and_com_stacked(request_reply,
                                         "CO<sub>2</sub>-äquivalente Emissionen bei temporal gekoppelter Kommunikation" + t_suffix,
                                         1400, 1400,
                                         "C:/Development/DHBW/dhbw-bachelorarbeit/assets/images/emissionen/emissionen-reqrep-gruppe-" + group + "-" + f_suffix,
                                         y_max_g)
    plot_by_hosting_type_and_com_stacked(messaging,
                                         "CO<sub>2</sub>-äquivalente Emissionen bei temporal entkoppelter Kommunikation" + t_suffix,
                                         1400, 1400,
                                         "C:/Development/DHBW/dhbw-bachelorarbeit/assets/images/emissionen/emissionen-messaging-gruppe-" + group + "-" + f_suffix,
                                         y_max_e)


# https://www.eea.europa.eu/data-and-maps/indicators/overview-of-the-electricity-production-3/assessment
eu_grid = 229
de_grid = 314 # 2020
fr_grid = 60
se_grid = 8

plot_emissions(de_grid, 4,
               " (Stromnetz: Deutschland, Serverbetriebsdauer: 4 Jahre)", "-de-4.png", 25, 30)
plot_emissions(se_grid, 4,
               " (Stromnetz: Schweden, Serverbetriebsdauer: 4 Jahre)", "-se-4.png", 12.5, 25)
plot_emissions(eu_grid, 4,
               " (Stromnetz: EU, Serverbetriebsdauer: 4 Jahre)", "-eu-4.png", 12.5, 25)

plot_emissions(de_grid, 6,
               " (Stromnetz: Deutschland, Serverbetriebsdauer: 6 Jahre)", "-de-6.png", 25, 30)
plot_emissions(se_grid, 6,
               " (Stromnetz: Schweden, Serverbetriebsdauer: 6 Jahre)", "-se-6.png", 25, 30)
plot_emissions(eu_grid, 6,
               " (Stromnetz: EU, Serverbetriebsdauer: 6 Jahre)", "-eu-6.png", 25, 30)


plot_group(eu_grid, 4,
           " (Stromnetz: EU, Serverbetriebsdauer: 4 Jahre)", "-eu-4.png", 7.5, 7.5, "mid")

plot_group(eu_grid, 6,
           " (Stromnetz: EU, Serverbetriebsdauer: 6 Jahre)", "-eu-6.png", 7.5, 7.5, "mid")

plot_group(de_grid, 4,
           " (Stromnetz: Deutschland, Serverbetriebsdauer: 4 Jahre)", "-de-4.png", 7.5, 7.5, "mid")

plot_group(se_grid, 4,
           " (Stromnetz: Schweden, Serverbetriebsdauer: 4 Jahre)", "-se-4.png", 7.5, 7.5, "mid")
