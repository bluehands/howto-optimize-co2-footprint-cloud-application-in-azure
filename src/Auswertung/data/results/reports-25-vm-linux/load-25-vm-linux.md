> test suite: `nbomber_default_test_suite_name`

> test name: `nbomber_default_test_name`

> scenario: `scenario`, duration: `02:00:00`, ok count: `359558`, fail count: `0`, all data: `3286.7997` MB MB

load simulation: `keep_constant`, copies: `10`, during: `02:00:00`
|step|ok stats|
|---|---|
|name|`get_products`|
|request count|all = `179784`, ok = `179784`, RPS = `25`|
|latency|min = `2.27`, mean = `6.75`, max = `525.44`, StdDev = `3.94`|
|latency percentile|50% = `5.55`, 75% = `8.86`, 95% = `12.8`, 99% = `16.59`|
|data transfer|min = `18.721` KB, mean = `18.727` KB, max = `18.721` KB, all = `3286.7997` MB|
|||
|name|`limiter`|
|request count|all = `179774`, ok = `179774`, RPS = `25`|
|latency|min = `0`, mean = `393.68`, max = `1140.98`, StdDev = `239.47`|
|latency percentile|50% = `352.26`, 75% = `589.31`, 95% = `803.84`, 99% = `928.77`|
> status codes for scenario: `scenario`

|status code|count|message|
|---|---|---|
|200|359558||

> hints:

|source|name|hint|
|---|---|---|
|Scenario|scenario|Step 'limiter' in scenario 'scenario' didn't track data transfer. In order to track data transfer, you should use Response.Ok(sizeInBytes: value)|
