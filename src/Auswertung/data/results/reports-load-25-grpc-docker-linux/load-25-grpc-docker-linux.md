> test suite: `nbomber_default_test_suite_name`

> test name: `nbomber_default_test_name`

> scenario: `scenario`, duration: `02:00:00`, ok count: `359564`, fail count: `0`, all data: `1964.0214` MB MB

load simulation: `keep_constant`, copies: `4`, during: `02:00:00`
|step|ok stats|
|---|---|
|name|`get_products`|
|request count|all = `179784`, ok = `179784`, RPS = `25`|
|latency|min = `7.81`, mean = `19.13`, max = `244.32`, StdDev = `7.15`|
|latency percentile|50% = `17.73`, 75% = `22.22`, 95% = `31.87`, 99% = `43.74`|
|data transfer|min = `11.187` KB, mean = `11.184` KB, max = `11.187` KB, all = `1964.0214` MB|
|||
|name|`limiter`|
|request count|all = `179780`, ok = `179780`, RPS = `25`|
|latency|min = `0`, mean = `141.03`, max = `701.63`, StdDev = `160.62`|
|latency percentile|50% = `70.78`, 75% = `177.41`, 95% = `517.12`, 99% = `627.71`|
> status codes for scenario: `scenario`

|status code|count|message|
|---|---|---|
|200|359564||

> hints:

|source|name|hint|
|---|---|---|
|Scenario|scenario|Step 'limiter' in scenario 'scenario' didn't track data transfer. In order to track data transfer, you should use Response.Ok(sizeInBytes: value)|
