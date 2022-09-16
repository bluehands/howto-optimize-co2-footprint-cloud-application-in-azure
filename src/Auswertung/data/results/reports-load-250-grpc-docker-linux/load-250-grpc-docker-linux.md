> test suite: `nbomber_default_test_suite_name`

> test name: `nbomber_default_test_name`

> scenario: `scenario`, duration: `02:00:00`, ok count: `1885400`, fail count: `0`, all data: `10298.3747` MB MB

load simulation: `keep_constant`, copies: `4`, during: `02:00:00`
|step|ok stats|
|---|---|
|name|`get_products`|
|request count|all = `942700`, ok = `942700`, RPS = `130.9`|
|latency|min = `9.2`, mean = `30.52`, max = `718.41`, StdDev = `11.13`|
|latency percentile|50% = `28.19`, 75% = `35.46`, 95% = `51.26`, 99% = `67.46`|
|data transfer|min = `11.187` KB, mean = `11.184` KB, max = `11.187` KB, all = `10298.3747` MB|
|||
|name|`limiter`|
|request count|all = `942700`, ok = `942700`, RPS = `130.9`|
|latency|min = `0`, mean = `0.02`, max = `40.08`, StdDev = `0.15`|
|latency percentile|50% = `0.01`, 75% = `0.02`, 95% = `0.04`, 99% = `0.1`|
> status codes for scenario: `scenario`

|status code|count|message|
|---|---|---|
|200|1885400||

> hints:

|source|name|hint|
|---|---|---|
|Scenario|scenario|Step 'limiter' in scenario 'scenario' didn't track data transfer. In order to track data transfer, you should use Response.Ok(sizeInBytes: value)|
