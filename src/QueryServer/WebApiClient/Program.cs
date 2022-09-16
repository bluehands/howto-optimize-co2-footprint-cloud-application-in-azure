// See https://aka.ms/new-console-template for more information

using ComposableAsync;
using NBomber.Configuration;
using NBomber.Contracts;
using NBomber.CSharp;
using NBomber.Plugins.Http.CSharp;
using RateLimiter;
using HttpClientFactory = NBomber.Plugins.Http.CSharp.HttpClientFactory;

// var builder = new ConfigurationBuilder()
//     .AddJsonFile($"appsettings.json", true, true);
// var config = builder.Build()!;
//
// var runConfiguration = config.GetSection("RunConfiguration").Get<RunConfiguration>();

var myHttpClient = new HttpClient();
myHttpClient.Timeout = TimeSpan.FromSeconds(200);
var httpFactory = HttpClientFactory.Create("my_http_factory", myHttpClient);


var timeConstraint = TimeLimiter.GetFromMaxCountByInterval(250, TimeSpan.FromSeconds(1));
//var timeConstraint = TimeLimiter.GetFromMaxCountByInterval(2, TimeSpan.FromMilliseconds(800));

var getProducts = Step.Create("get_products",
    clientFactory: httpFactory,
    execute: async context =>
    {
        var request = Http.CreateRequest("GET", "http://20.216.187.35/QueryProducts")
            .WithCheck(async response =>
            {
                var json = await response.Content.ReadAsByteArrayAsync();
                        
                return response.IsSuccessStatusCode
                    ? Response.Ok(json, statusCode: (int)response.StatusCode)
                    : Response.Fail(statusCode: (int)response.StatusCode);
            });
        var response = await Http.Send(request, context);
        return response;
    });

var limiter = Step.Create("limiter",
    execute: async _ =>
    {
        await timeConstraint;
        return Response.Ok(latencyMs: 0, statusCode: 200, sizeBytes: 0);
    }, TimeSpan.FromSeconds(100));

var scenario = ScenarioBuilder.CreateScenario("scenario", getProducts, limiter)
    .WithWarmUpDuration(TimeSpan.FromSeconds(4))
    .WithLoadSimulations(Simulation.KeepConstant(10, TimeSpan.FromHours(2)));
//.WithLoadSimulations(Simulation.InjectPerSec(10, TimeSpan.FromSeconds(60)));


NBomberRunner
    .RegisterScenarios(scenario)
    .WithReportFileName("load-250-vm-linux")
    .WithReportFolder("reports")
    .WithReportFormats(ReportFormat.Csv, ReportFormat.Html, ReportFormat.Md)
    .Run();