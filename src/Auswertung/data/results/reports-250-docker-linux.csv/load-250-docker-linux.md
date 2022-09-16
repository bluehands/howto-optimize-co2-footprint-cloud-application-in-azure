> test suite: `nbomber_default_test_suite_name`

> test name: `nbomber_default_test_name`

> scenario: `scenario`, duration: `02:00:00`, ok count: `3353190`, fail count: `16`, all data: `30651.4036` MB MB

load simulation: `keep_constant`, copies: `10`, during: `02:00:00`
|step|ok stats|
|---|---|
|name|`get_products`|
|request count|all = `1676611`, ok = `1676595`, RPS = `232.9`|
|latency|min = `6.37`, mean = `39.38`, max = `995.7`, StdDev = `16.56`|
|latency percentile|50% = `36.61`, 75% = `46.72`, 95% = `70.02`, 99% = `94.59`|
|data transfer|min = `18.721` KB, mean = `18.727` KB, max = `18.721` KB, all = `30651.4036` MB|
|||
|name|`limiter`|
|request count|all = `1676595`, ok = `1676595`, RPS = `232.9`|
|latency|min = `0`, mean = `3.54`, max = `152.59`, StdDev = `8.73`|
|latency percentile|50% = `0.03`, 75% = `0.22`, 95% = `23.42`, 99% = `39.71`|

|step|fail stats|
|---|---|
|name|`get_products`|
|request count|all = `1676611`, fail = `16`, RPS = `0`|
|latency|min = `998.1`, mean = `1000.9`, max = `1008.8`, StdDev = `3.01`|
|latency percentile|50% = `1000.45`, 75% = `1000.45`, 95% = `1008.64`, 99% = `1009.15`|
> status codes for scenario: `scenario`

|status code|count|message|
|---|---|---|
|200|3353190||
|-100|16|step timeout|

> hints:

|source|name|hint|
|---|---|---|
|Scenario|scenario|Step 'limiter' in scenario 'scenario' didn't track data transfer. In order to track data transfer, you should use Response.Ok(sizeInBytes: value)|
