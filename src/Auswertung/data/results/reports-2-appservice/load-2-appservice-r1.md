> test suite: `nbomber_default_test_suite_name`

> test name: `nbomber_default_test_name`

> scenario: `scenario`, duration: `02:00:00`, ok count: `35962`, fail count: `0`, all data: `334.9939` MB MB

load simulation: `keep_constant`, copies: `10`, during: `02:00:00`
|step|ok stats|
|---|---|
|name|`get_products`|
|request count|all = `17986`, ok = `17986`, RPS = `2.5`|
|latency|min = `5.56`, mean = `9.91`, max = `222.84`, StdDev = `4.32`|
|latency percentile|50% = `9.11`, 75% = `10.86`, 95% = `14.71`, 99% = `23.22`|
|data transfer|min = `19.072` KB, mean = `19.07` KB, max = `19.072` KB, all = `334.9939` MB|
|||
|name|`limiter`|
|request count|all = `17976`, ok = `17976`, RPS = `2.5`|
|latency|min = `5.09`, mean = `3994.38`, max = `4020.77`, StdDev = `62.7`|
|latency percentile|50% = `3995.65`, 75% = `3999.74`, 95% = `4005.89`, 99% = `4009.98`|
> status codes for scenario: `scenario`

|status code|count|message|
|---|---|---|
|200|35962||

> hints:

|source|name|hint|
|---|---|---|
|Scenario|scenario|Step 'limiter' in scenario 'scenario' didn't track data transfer. In order to track data transfer, you should use Response.Ok(sizeInBytes: value)|
