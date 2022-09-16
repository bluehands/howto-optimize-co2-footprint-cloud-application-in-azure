> test suite: `nbomber_default_test_suite_name`

> test name: `nbomber_default_test_name`

> scenario: `scenario`, duration: `02:00:00`, ok count: `35632`, fail count: `650`, all data: `325.8024` MB MB

load simulation: `keep_constant`, copies: `10`, during: `02:00:00`
|step|ok stats|
|---|---|
|name|`get_products`|
|request count|all = `18471`, ok = `17821`, RPS = `2.5`|
|latency|min = `9.27`, mean = `13.96`, max = `183.6`, StdDev = `4.23`|
|latency percentile|50% = `12.96`, 75% = `14.24`, 95% = `19.82`, 99% = `28.42`|
|data transfer|min = `18.721` KB, mean = `18.727` KB, max = `18.721` KB, all = `325.8024` MB|
|||
|name|`limiter`|
|request count|all = `17811`, ok = `17811`, RPS = `2.5`|
|latency|min = `0.14`, mean = `3990.66`, max = `4015.32`, StdDev = `63.81`|
|latency percentile|50% = `3993.6`, 75% = `3995.65`, 95% = `4001.79`, 99% = `4007.94`|

|step|fail stats|
|---|---|
|name|`get_products`|
|request count|all = `18471`, fail = `650`, RPS = `0.1`|
|latency|min = `994.42`, mean = `1001.72`, max = `1030.85`, StdDev = `3.3`|
|latency percentile|50% = `1001.47`, 75% = `1003.01`, 95% = `1008.13`, 99% = `1014.27`|
> status codes for scenario: `scenario`

|status code|count|message|
|---|---|---|
|200|35632||
|-100|650|step timeout|

> hints:

|source|name|hint|
|---|---|---|
|Scenario|scenario|Step 'limiter' in scenario 'scenario' didn't track data transfer. In order to track data transfer, you should use Response.Ok(sizeInBytes: value)|
