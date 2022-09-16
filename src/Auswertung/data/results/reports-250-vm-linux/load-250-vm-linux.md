> test suite: `nbomber_default_test_suite_name`

> test name: `nbomber_default_test_name`

> scenario: `scenario`, duration: `02:00:00`, ok count: `3594882`, fail count: `0`, all data: `32860.7208` MB MB

load simulation: `keep_constant`, copies: `10`, during: `02:00:00`
|step|ok stats|
|---|---|
|name|`get_products`|
|request count|all = `1797442`, ok = `1797442`, RPS = `249.6`|
|latency|min = `3.23`, mean = `9.78`, max = `245.94`, StdDev = `4.32`|
|latency percentile|50% = `9.67`, 75% = `11.58`, 95% = `15.78`, 99% = `21.5`|
|data transfer|min = `18.721` KB, mean = `18.727` KB, max = `18.721` KB, all = `32860.7208` MB|
|||
|name|`limiter`|
|request count|all = `1797440`, ok = `1797440`, RPS = `249.6`|
|latency|min = `0`, mean = `30.26`, max = `710.54`, StdDev = `58.8`|
|latency percentile|50% = `10.11`, 75% = `25.15`, 95% = `182.14`, 99% = `267.01`|
> status codes for scenario: `scenario`

|status code|count|message|
|---|---|---|
|200|3594882||

> hints:

|source|name|hint|
|---|---|---|
|Scenario|scenario|Step 'limiter' in scenario 'scenario' didn't track data transfer. In order to track data transfer, you should use Response.Ok(sizeInBytes: value)|
