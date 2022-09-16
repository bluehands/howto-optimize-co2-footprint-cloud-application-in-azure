# This is a sample Python script.
import json
import os
from expression import pipe
from expression.collections import seq
from expression.collections import Seq
import pandas as pd
import yaml
import matplotlib.pyplot as plt
import numpy as np

from load import load_all_as_pd

all_pd = load_all_as_pd()
all_pd.reset_index()

grouped = all_pd.groupby(["scenario"])

prefix = 'request'

x = grouped.agg({
    prefix + '_percentile_duration_25': 'mean',
    prefix + '_percentile_duration_50': 'mean',
    prefix + '_percentile_duration_75': 'mean',
    prefix + '_percentile_duration_90': 'mean',
    prefix + '_average_duration': 'mean',
    prefix + '_min_duration': 'min',
    prefix + '_max_duration': 'max',
})

stats = []
for index, row in x.iterrows():
    iqr = row[prefix + '_percentile_duration_75'] - row[prefix + '_percentile_duration_25']
    whislo = row[prefix + '_percentile_duration_25'] - 1.5 * iqr
    whishi = row[prefix + '_percentile_duration_75'] + 1.5 * iqr
    plot = {
        "label": index,  # not required
        "mean": row[prefix + '_average_duration'],  # not required
        "med": row[prefix + '_percentile_duration_50'],
        "q1": row[prefix + '_percentile_duration_25'],
        "q3": row[prefix + '_percentile_duration_75'],
        # "cilo": 5.3 # not required
        # "cihi": 5.7 # not required
        "whislo": row[prefix + '_min_duration'],  # required
        "whishi": whishi,  # required
        "fliers": []  # required if showfliers=True
    }
    stats.append(plot)

fs = 14  # fontsize

fig, axes = plt.subplots(nrows=1, ncols=1, figsize=(12, 12), sharey=True)
axes.bxp(stats)
axes.set_title('Dauer der Abhängigkeiten (Azure Tables, Azure SQL Database)', fontsize=fs)
axes.set_ylabel('Zeit Ms')
axes.set_xlabel('Szenario')

fig.autofmt_xdate(rotation=45)
plt.savefig('./data/out/abhängigkeiten.png')
plt.show()



#result.reset_index(inplace=True)

# with open('./data/out/table.json', 'w') as file:
#     file.write(result.to_json(orient='columns'))

print(all_pd)
# See PyCharm help at https://www.jetbrains.com/help/pycharm/
