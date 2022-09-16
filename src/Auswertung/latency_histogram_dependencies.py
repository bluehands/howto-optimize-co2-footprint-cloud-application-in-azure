import math
import os
import re

from expression import pipe
from expression.collections import seq
from expression.collections import Seq
import pandas as pd
import yaml
import matplotlib.pyplot as plt
from plotly.subplots import make_subplots
import plotly.graph_objects as go
from plotly.offline import plot
import numpy as np

from latency_dependencies import load_messaging_dependencies, load_request_reply_dependencies
from utils import rename_ids, render_boxplot


def render_histo(hist, dependencies, title, out, step, start, n, end, vspaceing):
    groups = hist.groupby('name')
    fig = make_subplots(
        rows=len(groups),
        cols=1,
        x_title='Dauer (Millisekunden)',
        y_title='Anzahl der Requests',
        vertical_spacing=vspaceing
    )  # go.Figure(layout=layout)

    row = 1
    for group_name, df_group in groups:
        bins = np.linspace(start, n, end)
        data = seq.of_iterable(df_group.itertuples()).map(lambda x: list(np.repeat(x[2], x[3]))).fold(
            lambda x, y: x + y,
            list())
        res = np.histogram(data, bins)
        fig.add_trace(
            go.Bar(
                x=res[1],
                y=res[0],
                name=group_name,
                # width=[0.5] * n,
            ),
            row=row,
            col=1,
        )
        deps = dependencies.groupby('name').get_group(group_name)

        xshift = 18
        yshift = -8
        fontsize = 7
        rotate = -45
        fig.add_vline(
            row=row,
            col=1,
            x=deps['percentile_DurationMs_25'].values[-1],
            line_width=1,
            line_dash="dash",
            line_color="green",
            annotation_text="p25",
            annotation=dict(
                yshift=xshift,
                xshift=yshift,
                textangle=rotate,
                font=dict(
                    size=fontsize,
                )
            ),
        )
        fig.add_vline(
            row=row,
            col=1,
            x=deps['percentile_DurationMs_50'].values[-1],
            line_width=1,
            line_dash="dash",
            line_color="green",
            annotation_text="p50",
            annotation=dict(
                yshift=xshift,
                xshift=yshift,
                textangle=rotate,
                font=dict(
                    size=fontsize,
                )
            ),
        )
        fig.add_vline(
            row=row,
            col=1,
            x=deps['percentile_DurationMs_75'].values[-1],
            line_width=1,
            line_dash="dash",
            line_color="green",
            annotation_text="p75",
            annotation=dict(
                yshift=xshift,
                xshift=yshift,
                textangle=rotate,
                font=dict(
                    size=fontsize,
                )
            ),
        )
        if deps['percentile_DurationMs_90'].values[-1] < n:
            fig.add_vline(
                row=row,
                col=1,
                x=deps['percentile_DurationMs_90'].values[-1],
                line_width=1,
                line_dash="dash",
                line_color="green",
                annotation_text="p90",
                annotation=dict(
                    yshift=xshift,
                    xshift=yshift,
                    textangle=rotate,
                    font=dict(
                        size=fontsize,
                    )
                ),
            )
        row += 1
    size = 1000
    fig.update_layout(
        width=size,
        height=size * 1.4142135,
        title=dict(
            text=title,
            y=0.96,  # new
            x=0.5,
            xanchor='center',
            yanchor='top',
            font=dict(
                size=14,
            ),
        ),
        font=dict(
            size=9,
        ),
        paper_bgcolor='rgba(0,0,0,0)',
        plot_bgcolor='rgba(0,0,0,0)'

    )
    fig.update_xaxes(
        dtick=step,
        tick0=start,
        tickmode='linear',
    )
    fig.update_yaxes(
        showgrid=False
    )
    fig.show()
    fig.write_image(out)


def load_dependencies_hist():
    tmp = pd.read_csv('./data/exports/dependencies_histo.csv')
    tmp.rename(columns={"_ResourceId": "name"}, inplace=True)
    tmp['name'] = tmp['name'].apply(rename_ids)
    return tmp


def load_messaging_dependencies_hist():
    tmp = pd.concat([
        pd.read_csv('./data/exports/dependencies_messaging_histo.csv'),
        pd.read_csv('./data/exports/dependencies_messaging_histo_1.csv')
    ])
    tmp.rename(columns={"_ResourceId": "name"}, inplace=True)
    tmp['name'] = tmp['name'].apply(rename_ids)
    return tmp


render_histo(
    load_messaging_dependencies_hist(),
    load_messaging_dependencies(),
    "Verteilung der Abhängigkeitsdauer",
    "./data/out/dependencies_messaging_histo.pdf",
    1,
    0,
    50,
    50 + 1,
    0
)
render_histo(
    load_dependencies_hist(),
    load_request_reply_dependencies(),
    "Verteilung der Abhängigkeitsdauer",
    "./data/out/dependencies_request_reply_histo.pdf",
    0.5,
    0,
    20,
    40 + 1,
    0.025
)
