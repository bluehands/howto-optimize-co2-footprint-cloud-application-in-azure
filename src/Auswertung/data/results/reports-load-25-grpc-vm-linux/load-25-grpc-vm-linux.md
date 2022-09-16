> test suite: `nbomber_default_test_suite_name`

> test name: `nbomber_default_test_name`

> scenario: `scenario`, duration: `02:00:00`, ok count: `359572`, fail count: `0`, all data: `1964.0651` MB MB

load simulation: `keep_constant`, copies: `4`, during: `02:00:00`
|step|ok stats|
|---|---|
|name|`get_products`|
|request count|all = `179788`, ok = `179788`, RPS = `25`|
|latency|min = `2.66`, mean = `5.29`, max = `121.4`, StdDev = `1.63`|
|latency percentile|50% = `4.97`, 75% = `5.95`, 95% = `7.77`, 99% = `10.2`|
|data transfer|min = `11.187` KB, mean = `11.184` KB, max = `11.187` KB, all = `1964.0651` MB|
|||
|name|`limiter`|
|request count|all = `179784`, ok = `179784`, RPS = `25`|
|latency|min = `0`, mean = `154.87`, max = `940.71`, StdDev = `176.45`|
|latency percentile|50% = `95.17`, 75% = `211.07`, 95% = `529.41`, 99% = `757.25`|
> status codes for scenario: `scenario`

|status code|count|message|
|---|---|---|
|200|359572||

> hints:

|source|name|hint|
|---|---|---|
|Scenario|scenario|Step 'limiter' in scenario 'scenario' didn't track data transfer. In order to track data transfer, you should use Response.Ok(sizeInBytes: value)|
