> test suite: `nbomber_default_test_suite_name`

> test name: `nbomber_default_test_name`

> scenario: `scenario`, duration: `02:00:00`, ok count: `35942`, fail count: `0`, all data: `328.6361` MB MB

load simulation: `keep_constant`, copies: `10`, during: `02:00:00`
|step|ok stats|
|---|---|
|name|`get_products`|
|request count|all = `17976`, ok = `17976`, RPS = `2.5`|
|latency|min = `8.4`, mean = `17.23`, max = `174.11`, StdDev = `5.34`|
|latency percentile|50% = `16.61`, 75% = `19.65`, 95% = `25.25`, 99% = `30.94`|
|data transfer|min = `18.721` KB, mean = `18.727` KB, max = `18.721` KB, all = `328.6361` MB|
|||
|name|`limiter`|
|request count|all = `17966`, ok = `17966`, RPS = `2.5`|
|latency|min = `3295.3`, mean = `3989.05`, max = `6460.75`, StdDev = `34.34`|
|latency percentile|50% = `3989.5`, 75% = `3993.6`, 95% = `3999.74`, 99% = `4005.89`|
> status codes for scenario: `scenario`

|status code|count|message|
|---|---|---|
|200|35942||

> hints:

|source|name|hint|
|---|---|---|
|Scenario|scenario|Step 'limiter' in scenario 'scenario' didn't track data transfer. In order to track data transfer, you should use Response.Ok(sizeInBytes: value)|
