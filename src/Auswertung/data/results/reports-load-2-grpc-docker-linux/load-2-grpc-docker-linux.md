> test suite: `nbomber_default_test_suite_name`

> test name: `nbomber_default_test_name`

> scenario: `scenario`, duration: `02:00:00`, ok count: `35948`, fail count: `0`, all data: `196.3759` MB MB

load simulation: `keep_constant`, copies: `4`, during: `02:00:00`
|step|ok stats|
|---|---|
|name|`get_products`|
|request count|all = `17976`, ok = `17976`, RPS = `2.5`|
|latency|min = `6.42`, mean = `13.2`, max = `169.06`, StdDev = `4.81`|
|latency percentile|50% = `12.36`, 75% = `14.77`, 95% = `20.9`, 99% = `28.98`|
|data transfer|min = `11.187` KB, mean = `11.184` KB, max = `11.187` KB, all = `196.3759` MB|
|||
|name|`limiter`|
|request count|all = `17972`, ok = `17972`, RPS = `2.5`|
|latency|min = `822.82`, mean = `1589.01`, max = `1632.75`, StdDev = `9.74`|
|latency percentile|50% = `1590.27`, 75% = `1592.32`, 95% = `1596.42`, 99% = `1599.49`|
> status codes for scenario: `scenario`

|status code|count|message|
|---|---|---|
|200|35948||

> hints:

|source|name|hint|
|---|---|---|
|Scenario|scenario|Step 'limiter' in scenario 'scenario' didn't track data transfer. In order to track data transfer, you should use Response.Ok(sizeInBytes: value)|
