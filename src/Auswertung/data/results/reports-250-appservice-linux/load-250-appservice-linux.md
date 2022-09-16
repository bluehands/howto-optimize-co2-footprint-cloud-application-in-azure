> test suite: `nbomber_default_test_suite_name`

> test name: `nbomber_default_test_name`

> scenario: `scenario`, duration: `02:00:00`, ok count: `3558246`, fail count: `62`, all data: `32525.9419` MB MB

load simulation: `keep_constant`, copies: `10`, during: `02:00:00`
|step|ok stats|
|---|---|
|name|`get_products`|
|request count|all = `1779188`, ok = `1779126`, RPS = `247.1`|
|latency|min = `5.31`, mean = `22.26`, max = `892.22`, StdDev = `12.3`|
|latency percentile|50% = `19.93`, 75% = `27.23`, 95% = `42.27`, 99% = `64.45`|
|data transfer|min = `18.721` KB, mean = `18.727` KB, max = `18.721` KB, all = `32525.8688` MB|
|||
|name|`limiter`|
|request count|all = `1779120`, ok = `1779120`, RPS = `247.1`|
|latency|min = `0`, mean = `18.19`, max = `182.93`, StdDev = `17.33`|
|latency percentile|50% = `15.41`, 75% = `25.55`, 95% = `49.89`, 99% = `83.07`|

|step|fail stats|
|---|---|
|name|`get_products`|
|request count|all = `1779188`, fail = `62`, RPS = `0`|
|latency|min = `9.7`, mean = `493.77`, max = `1003.47`, StdDev = `490.49`|
|latency percentile|50% = `40.64`, 75% = `1000.45`, 95% = `1003.01`, 99% = `1003.01`|
|data transfer|min = `2.34` KB, mean = `2.341` KB, max = `2.34` KB, all = `0.0731` MB|
> status codes for scenario: `scenario`

|status code|count|message|
|---|---|---|
|200|3558246||
|-100|30|step timeout|
|403|32||

> hints:

|source|name|hint|
|---|---|---|
|Scenario|scenario|Step 'limiter' in scenario 'scenario' didn't track data transfer. In order to track data transfer, you should use Response.Ok(sizeInBytes: value)|
