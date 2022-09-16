> test suite: `nbomber_default_test_suite_name`

> test name: `nbomber_default_test_name`

> scenario: `scenario`, duration: `02:00:00`, ok count: `359538`, fail count: `2`, all data: `3286.6166` MB MB

load simulation: `keep_constant`, copies: `10`, during: `02:00:00`
|step|ok stats|
|---|---|
|name|`get_products`|
|request count|all = `179776`, ok = `179774`, RPS = `25`|
|latency|min = `5.46`, mean = `21.07`, max = `979.76`, StdDev = `13.18`|
|latency percentile|50% = `17.97`, 75% = `27.04`, 95% = `42.3`, 99% = `59.74`|
|data transfer|min = `18.679` KB, mean = `18.726` KB, max = `18.721` KB, all = `3286.6166` MB|
|||
|name|`limiter`|
|request count|all = `179764`, ok = `179764`, RPS = `25`|
|latency|min = `0`, mean = `379.4`, max = `947.64`, StdDev = `304.14`|
|latency percentile|50% = `264.45`, 75% = `690.69`, 95% = `860.67`, 99% = `907.26`|

|step|fail stats|
|---|---|
|name|`get_products`|
|request count|all = `179776`, fail = `2`, RPS = `0`|
|latency|min = `1000.43`, mean = `1000.7`, max = `1001.35`, StdDev = `0.51`|
|latency percentile|50% = `1000.45`, 75% = `1001.47`, 95% = `1001.47`, 99% = `1001.47`|
> status codes for scenario: `scenario`

|status code|count|message|
|---|---|---|
|200|359538||
|-100|2|step timeout|

> hints:

|source|name|hint|
|---|---|---|
|Scenario|scenario|Step 'limiter' in scenario 'scenario' didn't track data transfer. In order to track data transfer, you should use Response.Ok(sizeInBytes: value)|
