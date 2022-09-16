> test suite: `nbomber_default_test_suite_name`

> test name: `nbomber_default_test_name`

> scenario: `scenario`, duration: `02:00:00`, ok count: `359475`, fail count: `21`, all data: `3286.0319` MB MB

load simulation: `keep_constant`, copies: `10`, during: `02:00:00`
|step|ok stats|
|---|---|
|name|`get_products`|
|request count|all = `179763`, ok = `179742`, RPS = `25`|
|latency|min = `7.28`, mean = `21.53`, max = `355.21`, StdDev = `10.48`|
|latency percentile|50% = `19.06`, 75% = `27.73`, 95% = `38.85`, 99% = `50.78`|
|data transfer|min = `18.721` KB, mean = `18.727` KB, max = `18.721` KB, all = `3286.0319` MB|
|||
|name|`limiter`|
|request count|all = `179733`, ok = `179733`, RPS = `25`|
|latency|min = `0`, mean = `378.91`, max = `953.48`, StdDev = `257.45`|
|latency percentile|50% = `283.14`, 75% = `648.7`, 95% = `785.92`, 99% = `898.56`|

|step|fail stats|
|---|---|
|name|`get_products`|
|request count|all = `179763`, fail = `21`, RPS = `0`|
|latency|min = `997.92`, mean = `1002.02`, max = `1005.73`, StdDev = `2.47`|
|latency percentile|50% = `1002.5`, 75% = `1004.54`, 95% = `1005.57`, 99% = `1006.08`|
> status codes for scenario: `scenario`

|status code|count|message|
|---|---|---|
|200|359475||
|-100|21|step timeout|

> hints:

|source|name|hint|
|---|---|---|
|Scenario|scenario|Step 'limiter' in scenario 'scenario' didn't track data transfer. In order to track data transfer, you should use Response.Ok(sizeInBytes: value)|
