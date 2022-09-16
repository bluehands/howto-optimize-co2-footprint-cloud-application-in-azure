import json
import os
import re

from expression import pipe
from expression import pipe
from expression.collections import seq
from expression.collections import Seq
import pandas as pd
from load import load_folders
from bs4 import BeautifulSoup



def load_html_files(folder_item):
    return list(
        pipe(
            seq.of_iterable(os.scandir(folder_item.path)),
            seq.filter(lambda e: e.name.endswith(".html")),
            seq.map(lambda file: (folder_item.name, file))
        )
    )


def trim_js(text):
    res = re.sub(r'\s*// example of view model structure can be found in data.js\s*', '', text)
    res = re.sub(r'const viewModel = ', '', res)
    return re.sub(r';\n*\s*initApp\(\'#app\',\s*viewModel\);', '', res)


def load_bomber_final_stats_from(scenario):
    name, json_text = scenario
    scenario = json.loads(json_text)
    final_stats = scenario['FinalStats']['ScenarioStats'][0]['StepStats'][0]
    result = {
        'name': name,
        'min': final_stats['Ok']['Latency']['MinMs'],
        'mean': final_stats['Ok']['Latency']['MeanMs'],
        'max': final_stats['Ok']['Latency']['MaxMs'],
        'p50': final_stats['Ok']['Latency']['Percent50'],
        'p75': final_stats['Ok']['Latency']['Percent75'],
        'p95': final_stats['Ok']['Latency']['Percent95'],
        'p99': final_stats['Ok']['Latency']['Percent99'],
        'stddev': final_stats['Ok']['Latency']['StdDev'],
        'data_bytes': final_stats['Ok']['DataTransfer']['AllBytes'],
        'rcount': final_stats['Ok']['Request']['Count'],
        'rps': final_stats['Ok']['Request']['RPS'],
        'rerr': final_stats['Fail']['Request']['Count'],
    }
    return name, result

def load_bomber_scenario_stats_from(scenario):
    name, json_text = scenario
    scenario = json.loads(json_text)
    final_stats = scenario['TimeLineHistory']['ScenarioStats'][0]['StepStats'][0]
    result = {
        'name': name,
        'min': final_stats['Ok']['Latency']['MinMs'],
        'mean': final_stats['Ok']['Latency']['MeanMs'],
        'max': final_stats['Ok']['Latency']['MaxMs'],
        'p50': final_stats['Ok']['Latency']['Percent50'],
        'p75': final_stats['Ok']['Latency']['Percent75'],
        'p95': final_stats['Ok']['Latency']['Percent95'],
        'p99': final_stats['Ok']['Latency']['Percent99'],
        'stddev': final_stats['Ok']['Latency']['StdDev'],
        'data_bytes': final_stats['Ok']['DataTransfer']['AllBytes'],
        'rcount': final_stats['Ok']['Request']['Count'],
        'rps': final_stats['Ok']['Request']['RPS'],
        'rerr': final_stats['Fail']['Request']['Count'],
    }
    return name, result


def load_from_html(scenario):
    name, path = scenario
    with open(path) as fp:
        file_soup = BeautifulSoup(fp, 'html.parser')

    scripts = file_soup.findAll('script')
    last = scripts[-1]
    return name, trim_js(last.text)


def load_nbomber_final_stats():
    return pipe(
        load_folders(),
        seq.map(load_html_files),
        seq.filter(lambda e: len(e) != 0),
        seq.fold(lambda x, y: x + y, list()),
        seq.map(load_from_html),
        seq.map(load_bomber_final_stats_from),
        seq.map(lambda t: t[1])
    )




res = pd.DataFrame(load_nbomber_final_stats())

print("")
