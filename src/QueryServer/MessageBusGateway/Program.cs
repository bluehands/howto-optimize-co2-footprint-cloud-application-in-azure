using System.Reactive.Subjects;
using Azure.Messaging.ServiceBus;
using MessageBusGateway.Worker;
using MessagesContract;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton(
            new ServiceBusClient(
                context
                    .Configuration
                    .GetSection("ServiceBus")
                    .GetValue<string>("ConnectionString")));

        services.AddSingleton(new Subject<ProductsMessage>());

        services.Configure<QueueConfiguration>(
            context
                .Configuration
                .GetSection("ServiceBus")
                .GetSection("QueueConfiguration"));

        services.AddLogging(builder => builder.AddConsole());
        services.AddHostedService<QueryResponseWorker>();
        services.AddHostedService<FakeGatewayWorker>();
    })
    .Build();

await host.RunAsync();