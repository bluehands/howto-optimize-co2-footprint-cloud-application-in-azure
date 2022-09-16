using Application;
using Azure.Messaging.ServiceBus;
using AzureTableStorage;
using BasketCommandAppservice;

var builder = WebApplication.CreateBuilder(args);


var configurationSection = builder.Configuration.GetSection("WorkerConfiguration");
builder.Services.Configure<WorkerConfiguration>(configurationSection);
builder.Services.AddSingleton(
    new ServiceBusClient(builder.Configuration["ServiceBusConnectionString"]));

builder.Services.AddHostedService<AddItemToBasketWorker>();

builder.Services.RegisterAzureTableRepositories(builder.Configuration["TableConnectionString"],
    builder.Configuration["TableName"]);

builder.Services.AddTransient<StoreBasket>();

builder.Services.AddApplicationInsightsTelemetry();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();