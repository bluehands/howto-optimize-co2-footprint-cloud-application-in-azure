> test suite: `nbomber_default_test_suite_name`

> test name: `nbomber_default_test_name`

> scenario: `scenario`, duration: `02:00:00`, ok count: `3495384`, fail count: `142`, all data: `31879.5433` MB MB

load simulation: `keep_constant`, copies: `10`, during: `02:00:00`
|step|ok stats|
|---|---|
|name|`get_products`|
|request count|all = `1747835`, ok = `1747693`, RPS = `242.7`|
|latency|min = `16.06`, mean = `34.29`, max = `994.23`, StdDev = `18.75`|
|latency percentile|50% = `29.66`, 75% = `36.26`, 95% = `58.3`, 99% = `101.31`|
|data transfer|min = `18.637` KB, mean = `18.679` KB, max = `18.679` KB, all = `31879.5433` MB|
|||
|name|`limiter`|
|request count|all = `1747691`, ok = `1747691`, RPS = `242.7`|
|latency|min = `0`, mean = `6.81`, max = `200.56`, StdDev = `12.09`|
|latency percentile|50% = `2.13`, 75% = `9.38`, 95% = `26.05`, 99% = `61.63`|

|step|fail stats|
|---|---|
|name|`get_products`|
|request count|all = `1747835`, fail = `142`, RPS = `0`|
|latency|min = `994.01`, mean = `1000.32`, max = `1010.98`, StdDev = `2.74`|
|latency percentile|50% = `1000.45`, 75% = `1000.96`, 95% = `1007.1`, 99% = `1010.18`|
> status codes for scenario: `scenario`

|status code|count|message|
|---|---|---|
|200|3495384||
|-100|142|step timeout|

> hints:

|source|name|hint|
|---|---|---|
|Scenario|scenario|Step 'limiter' in scenario 'scenario' didn't track data transfer. In order to track data transfer, you should use Response.Ok(sizeInBytes: value)|
