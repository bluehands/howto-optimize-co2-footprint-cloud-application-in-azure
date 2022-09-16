> test suite: `nbomber_default_test_suite_name`

> test name: `nbomber_default_test_name`

> scenario: `scenario`, duration: `02:00:00`, ok count: `3594266`, fail count: `0`, all data: `19632.4906` MB MB

load simulation: `keep_constant`, copies: `4`, during: `02:00:00`
|step|ok stats|
|---|---|
|name|`get_products`|
|request count|all = `1797133`, ok = `1797133`, RPS = `249.6`|
|latency|min = `3.3`, mean = `5.79`, max = `213.18`, StdDev = `1.55`|
|latency percentile|50% = `5.52`, 75% = `6.32`, 95% = `7.86`, 99% = `12.54`|
|data transfer|min = `11.187` KB, mean = `11.184` KB, max = `11.187` KB, all = `19632.4906` MB|
|||
|name|`limiter`|
|request count|all = `1797133`, ok = `1797133`, RPS = `249.6`|
|latency|min = `0`, mean = `10.22`, max = `639.98`, StdDev = `22.88`|
|latency percentile|50% = `2.34`, 75% = `10.21`, 95% = `48.35`, 99% = `102.72`|
> status codes for scenario: `scenario`

|status code|count|message|
|---|---|---|
|200|3594266||

> hints:

|source|name|hint|
|---|---|---|
|Scenario|scenario|Step 'limiter' in scenario 'scenario' didn't track data transfer. In order to track data transfer, you should use Response.Ok(sizeInBytes: value)|
