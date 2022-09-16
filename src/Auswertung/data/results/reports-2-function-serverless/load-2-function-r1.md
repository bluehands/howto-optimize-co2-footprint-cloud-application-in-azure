> test suite: `nbomber_default_test_suite_name`

> test name: `nbomber_default_test_name`

> scenario: `scenario`, duration: `02:00:00`, ok count: `35942`, fail count: `61`, all data: `327.8989` MB MB

load simulation: `keep_constant`, copies: `10`, during: `02:00:00`
|step|ok stats|
|---|---|
|name|`get_products`|
|request count|all = `18037`, ok = `17976`, RPS = `2.5`|
|latency|min = `17.46`, mean = `33.07`, max = `959.38`, StdDev = `29.81`|
|latency percentile|50% = `27.98`, 75% = `32.14`, 95% = `53.6`, 99% = `119.74`|
|data transfer|min = `18.679` KB, mean = `18.68` KB, max = `18.679` KB, all = `327.8989` MB|
|||
|name|`limiter`|
|request count|all = `17966`, ok = `17966`, RPS = `2.5`|
|latency|min = `5.27`, mean = `3970.03`, max = `4003.39`, StdDev = `80.78`|
|latency percentile|50% = `3977.22`, 75% = `3981.31`, 95% = `3989.5`, 99% = `3993.6`|

|step|fail stats|
|---|---|
|name|`get_products`|
|request count|all = `18037`, fail = `61`, RPS = `0`|
|latency|min = `998`, mean = `1001.56`, max = `1008.26`, StdDev = `2.24`|
|latency percentile|50% = `1000.96`, 75% = `1004.03`, 95% = `1005.06`, 99% = `1005.06`|
> status codes for scenario: `scenario`

|status code|count|message|
|---|---|---|
|200|35942||
|-100|61|step timeout|

> hints:

|source|name|hint|
|---|---|---|
|Scenario|scenario|Step 'limiter' in scenario 'scenario' didn't track data transfer. In order to track data transfer, you should use Response.Ok(sizeInBytes: value)|
