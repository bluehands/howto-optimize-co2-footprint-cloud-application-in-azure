> test suite: `nbomber_default_test_suite_name`

> test name: `nbomber_default_test_name`

> scenario: `scenario`, duration: `02:00:00`, ok count: `3589395`, fail count: `13`, all data: `33414.8094` MB MB

load simulation: `keep_constant`, copies: `10`, during: `02:00:00`
|step|ok stats|
|---|---|
|name|`get_products`|
|request count|all = `1794715`, ok = `1794702`, RPS = `249.3`|
|latency|min = `5.88`, mean = `14.2`, max = `986.95`, StdDev = `8.28`|
|latency percentile|50% = `12.81`, 75% = `16.28`, 95% = `24.7`, 99% = `36.06`|
|data transfer|min = `19.065` KB, mean = `19.07` KB, max = `19.065` KB, all = `33414.8094` MB|
|||
|name|`limiter`|
|request count|all = `1794693`, ok = `1794693`, RPS = `249.3`|
|latency|min = `0`, mean = `25.9`, max = `389.24`, StdDev = `26.38`|
|latency percentile|50% = `19.66`, 75% = `36.67`, 95% = `73.47`, 99% = `119.74`|

|step|fail stats|
|---|---|
|name|`get_products`|
|request count|all = `1794715`, fail = `13`, RPS = `0`|
|latency|min = `998.12`, mean = `1000.35`, max = `1001.67`, StdDev = `0.88`|
|latency percentile|50% = `1000.45`, 75% = `1000.96`, 95% = `1001.98`, 99% = `1001.98`|
> status codes for scenario: `scenario`

|status code|count|message|
|---|---|---|
|200|3589395||
|-100|13|step timeout|

> hints:

|source|name|hint|
|---|---|---|
|Scenario|scenario|Step 'limiter' in scenario 'scenario' didn't track data transfer. In order to track data transfer, you should use Response.Ok(sizeInBytes: value)|
