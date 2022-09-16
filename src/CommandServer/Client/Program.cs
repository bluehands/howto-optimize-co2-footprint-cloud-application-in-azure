using Azure.Messaging.ServiceBus;
using Client;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.Configure<ClientConfiguration>(context.Configuration.GetSection("ClientConfiguration"));
        services.AddHostedService<ClientWorker>();
        services.AddSingleton(
            new ServiceBusClient(context.Configuration.GetSection("ClientConfiguration")
                .GetValue<string>("ConnectionString")));
    })
    .Build();

await host.RunAsync();