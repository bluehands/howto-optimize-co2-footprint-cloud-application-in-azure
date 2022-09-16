> test suite: `nbomber_default_test_suite_name`

> test name: `nbomber_default_test_name`

> scenario: `scenario`, duration: `02:00:00`, ok count: `359470`, fail count: `25`, all data: `3346.1724` MB MB

load simulation: `keep_constant`, copies: `10`, during: `02:00:00`
|step|ok stats|
|---|---|
|name|`get_products`|
|request count|all = `179765`, ok = `179740`, RPS = `25`|
|latency|min = `4.9`, mean = `12.89`, max = `552.01`, StdDev = `6.68`|
|latency percentile|50% = `11.54`, 75% = `15.18`, 95% = `21.81`, 99% = `29.41`|
|data transfer|min = `19.021` KB, mean = `19.069` KB, max = `19.063` KB, all = `3346.1610` MB|
|||
|name|`limiter`|
|request count|all = `179730`, ok = `179730`, RPS = `25`|
|latency|min = `0`, mean = `387.55`, max = `975.13`, StdDev = `390.55`|
|latency percentile|50% = `121.02`, 75% = `853.5`, 95% = `945.15`, 99% = `964.61`|

|step|fail stats|
|---|---|
|name|`get_products`|
|request count|all = `179765`, fail = `25`, RPS = `0`|
|latency|min = `11.27`, mean = `805.29`, max = `1001.8`, StdDev = `389.46`|
|latency percentile|50% = `999.94`, 75% = `1000.96`, 95% = `1001.47`, 99% = `1001.98`|
|data transfer|min = `2.34` KB, mean = `2.341` KB, max = `2.34` KB, all = `0.0114` MB|
> status codes for scenario: `scenario`

|status code|count|message|
|---|---|---|
|200|359470||
|-100|20|step timeout|
|403|5||

> hints:

|source|name|hint|
|---|---|---|
|Scenario|scenario|Step 'limiter' in scenario 'scenario' didn't track data transfer. In order to track data transfer, you should use Response.Ok(sizeInBytes: value)|
