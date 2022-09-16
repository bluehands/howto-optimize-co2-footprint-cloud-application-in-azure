> test suite: `nbomber_default_test_suite_name`

> test name: `nbomber_default_test_name`

> scenario: `scenario`, duration: `02:00:00`, ok count: `35942`, fail count: `0`, all data: `328.6361` MB MB

load simulation: `keep_constant`, copies: `10`, during: `02:00:00`
|step|ok stats|
|---|---|
|name|`get_products`|
|request count|all = `17976`, ok = `17976`, RPS = `2.5`|
|latency|min = `2.44`, mean = `4.45`, max = `125.37`, StdDev = `1.94`|
|latency percentile|50% = `4.36`, 75% = `4.81`, 95% = `5.59`, 99% = `6.56`|
|data transfer|min = `18.721` KB, mean = `18.727` KB, max = `18.721` KB, all = `328.6361` MB|
|||
|name|`limiter`|
|request count|all = `17966`, ok = `17966`, RPS = `2.5`|
|latency|min = `3561.9`, mean = `4001.98`, max = `6760.8`, StdDev = `38.5`|
|latency percentile|50% = `4001.79`, 75% = `4005.89`, 95% = `4009.98`, 99% = `4016.13`|
> status codes for scenario: `scenario`

|status code|count|message|
|---|---|---|
|200|35942||

> hints:

|source|name|hint|
|---|---|---|
|Scenario|scenario|Step 'limiter' in scenario 'scenario' didn't track data transfer. In order to track data transfer, you should use Response.Ok(sizeInBytes: value)|
