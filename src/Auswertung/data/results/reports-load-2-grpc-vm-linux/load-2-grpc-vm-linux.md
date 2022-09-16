> test suite: `nbomber_default_test_suite_name`

> test name: `nbomber_default_test_name`

> scenario: `scenario`, duration: `02:00:00`, ok count: `35946`, fail count: `0`, all data: `196.3650` MB MB

load simulation: `keep_constant`, copies: `4`, during: `02:00:00`
|step|ok stats|
|---|---|
|name|`get_products`|
|request count|all = `17975`, ok = `17975`, RPS = `2.5`|
|latency|min = `2.7`, mean = `4.47`, max = `130.15`, StdDev = `1.8`|
|latency percentile|50% = `4.37`, 75% = `4.72`, 95% = `5.54`, 99% = `7.11`|
|data transfer|min = `11.187` KB, mean = `11.184` KB, max = `11.187` KB, all = `196.3650` MB|
|||
|name|`limiter`|
|request count|all = `17971`, ok = `17971`, RPS = `2.5`|
|latency|min = `1358.58`, mean = `1597.91`, max = `2157.62`, StdDev = `7.24`|
|latency percentile|50% = `1598.46`, 75% = `1600.51`, 95% = `1603.58`, 99% = `1606.66`|
> status codes for scenario: `scenario`

|status code|count|message|
|---|---|---|
|200|35946||

> hints:

|source|name|hint|
|---|---|---|
|Scenario|scenario|Step 'limiter' in scenario 'scenario' didn't track data transfer. In order to track data transfer, you should use Response.Ok(sizeInBytes: value)|
