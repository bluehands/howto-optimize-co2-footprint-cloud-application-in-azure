// See https://aka.ms/new-console-template for more information

using ComposableAsync;
using Grpc.Net.Client;
using GrpcQueryServer;
using NBomber.Configuration;
using NBomber.Contracts;
using NBomber.CSharp;
using RateLimiter;


var address = "https://load-2-grpc.azurewebsites.net";
var fileName = "load-2-grpc-appservice-linux";
var maxCount = 2;
var perSecond = TimeSpan.FromMilliseconds(800);
//var perSecond = TimeSpan.FromSeconds(1);

var runTime = TimeSpan.FromHours(2);

var timeConstraint = TimeLimiter.GetFromMaxCountByInterval(maxCount, perSecond);


var channel = GrpcChannel.ForAddress(address, new GrpcChannelOptions()
{
    HttpHandler = new HttpClientHandler()
    {
        ServerCertificateCustomValidationCallback = (message, certificate2, arg3, arg4) => true
    }
});


var getProducts = Step.Create("get_products",
    execute: async _ =>
    {
        try
        {
            var client = new ProductCatalog.ProductCatalogClient(channel);
            var response = await client.QueryProductsAsync(new ProductsRequest());
            return Response.Ok(sizeBytes: response.CalculateSize(), statusCode: 200);
        }
        catch (Exception e)
        {
            return Response.Fail(e);
        }
    });

var limiter = Step.Create("limiter",
    execute: async _ =>
    {
        await timeConstraint;
        return Response.Ok(latencyMs: 0, statusCode: 200, sizeBytes: 0);
    }, TimeSpan.FromSeconds(100));


var scenario = ScenarioBuilder.CreateScenario("scenario", getProducts, limiter)
    .WithWarmUpDuration(TimeSpan.FromSeconds(4))
    .WithLoadSimulations(Simulation.KeepConstant(4, runTime));

NBomberRunner
    .RegisterScenarios(scenario)
    .WithReportFileName(fileName)
    .WithReportFolder($"reports-{fileName}")
    .WithReportFormats(ReportFormat.Csv, ReportFormat.Html, ReportFormat.Md)
    .Run();