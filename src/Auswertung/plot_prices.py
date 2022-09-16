import plotly.graph_objects as go

# Create random data with numpy
import numpy as np

# Create traces
fig = go.Figure()
fig.add_trace(
    go.Scatter(
        x=[2.5, 25, 250],
        y=[1.19, 21.41, 304.31],
        mode='lines+markers',
        name='Azure Function (Azure Service Bus)'
    )
)

fig.add_trace(
    go.Scatter(
        x=[2.5, 25, 250],
        y=[1.19, 12.65, 150.93],
        mode='lines+markers',
        name='Azure Function (Web-API)'
    )
)

fig.add_trace(
    go.Scatter(
        x=[2.5, 25, 250],
        y=[126.63, 126.63, 126.63],
        mode='lines+markers',
        name='Azure App Serivce'
    )
)

fig.add_trace(
    go.Scatter(
        x=[2.5, 25, 250],
        y=[95.33, 95.33, 95.33],
        mode='lines+markers',
        name='Azure Container Instance'
    )
)

fig.add_trace(
    go.Scatter(
        x=[2.5, 25, 250],
        y=[96.75, 96.75, 96.75],
        mode='lines+markers',
        name='Azure VM'
    )
)

fig.update_layout(
        autosize=False,
        width=1400,
        height=1000,  # * 1.4142135,
        title=dict(
            text="Preisabschätzung im Verhältnis zur Auslastung",
            y=0.97,  # new
            x=0.5,
            xanchor='center',
            yanchor='top',
            font=dict(
                size=28,
            ),
        ),
        yaxis_title="Europreis / Monat",
        xaxis_title="Auslastung (Anfragen / Sekunde)",
        font=dict(
            size=20,
        ),
      #  yaxis_range=[0, ymax]
        # paper_bgcolor='rgba(0,0,0,0)',
        # plot_bgcolor='rgba(0,0,0,0)'

    )

fig.write_image("C:/Development/DHBW/dhbw-bachelorarbeit/assets/images/preise.png")
fig.show()
