using Application;
using Application.Services;
using Azure.Messaging.ServiceBus;
using Database;
using InMemoryDatabase;
using MessageBusQueryWorker.Worker;
using Microsoft.EntityFrameworkCore;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddTransient<QueryProducts>();
        services.AddTransient<CalculateDiscount>();
        services.AddTransient<CalculateVatPrice>();
        services.AddDbContext<ProductContext>(optionsBuilder =>
        {
            optionsBuilder.UseSqlServer(context.Configuration.GetConnectionString("badb"));
        });
        services.AddScoped<IReadonlyProductRepository, InMemoryReadonlyProductRepository>();
        services.AddHostedService<QueryServiceWorker>();
        // services.AddAzureClients(builder =>
        //     builder
        //         .AddServiceBusClient(context.Configuration.GetSection("ServiceBus"))
        //         .WithCredential(new Azure.Identity.DefaultAzureCredential())
        // );
        services.AddSingleton(new ServiceBusClient(
            context
                .Configuration
                .GetSection("ServiceBus")
                .GetValue<string>("ConnectionString"))
        );
        services.Configure<QueueConfiguration>(context.Configuration.GetSection("ServiceBus")
            .GetSection("QueueConfiguration"));
        services.AddLogging(builder => builder.AddConsole());
    })
    .Build();


await host.RunAsync();