using Application;
using Azure.Messaging.ServiceBus;
using AzureTableStorage;
using BasketCommandWorker;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((builder, services) =>
    {
        var configurationSection = builder.Configuration.GetSection("WorkerConfiguration");
        services.Configure<WorkerConfiguration>(configurationSection);
        services.AddSingleton(
            new ServiceBusClient(builder.Configuration["ServiceBusConnectionString"]));

        services.AddHostedService<AddItemToBasketWorker>();

        services.RegisterAzureTableRepositories(builder.Configuration["TableConnectionString"], builder.Configuration["TableName"]);
        services.AddApplicationInsightsTelemetryWorkerService();

        services.AddTransient<StoreBasket>();
    })
    .Build();

await host.RunAsync();