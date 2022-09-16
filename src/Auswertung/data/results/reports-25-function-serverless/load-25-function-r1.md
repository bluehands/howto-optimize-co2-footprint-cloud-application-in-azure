> test suite: `nbomber_default_test_suite_name`

> test name: `nbomber_default_test_name`

> scenario: `scenario`, duration: `02:00:00`, ok count: `356240`, fail count: `855`, all data: `3249.1837` MB MB

load simulation: `keep_constant`, copies: `10`, during: `02:00:00`
|step|ok stats|
|---|---|
|name|`get_products`|
|request count|all = `178980`, ok = `178125`, RPS = `24.7`|
|latency|min = `15.67`, mean = `28.47`, max = `977.83`, StdDev = `11.13`|
|latency percentile|50% = `26.34`, 75% = `30.51`, 95% = `42.98`, 99% = `61.12`|
|data transfer|min = `18.679` KB, mean = `18.68` KB, max = `18.679` KB, all = `3249.1654` MB|
|||
|name|`limiter`|
|request count|all = `178115`, ok = `178115`, RPS = `24.7`|
|latency|min = `0`, mean = `371.93`, max = `917.91`, StdDev = `206.89`|
|latency percentile|50% = `334.59`, 75% = `569.34`, 95% = `705.54`, 99% = `754.69`|

|step|fail stats|
|---|---|
|name|`get_products`|
|request count|all = `178980`, fail = `855`, RPS = `0.1`|
|latency|min = `12.3`, mean = `787.53`, max = `1015.43`, StdDev = `403.99`|
|latency percentile|50% = `999.94`, 75% = `1001.47`, 95% = `1003.52`, 99% = `1005.57`|
|data transfer|min = `0.101` KB, mean = `0.101` KB, max = `0.101` KB, all = `0.0183` MB|
> status codes for scenario: `scenario`

|status code|count|message|
|---|---|---|
|200|356240||
|-100|669|step timeout|
|429|186||

> hints:

|source|name|hint|
|---|---|---|
|Scenario|scenario|Step 'limiter' in scenario 'scenario' didn't track data transfer. In order to track data transfer, you should use Response.Ok(sizeInBytes: value)|
