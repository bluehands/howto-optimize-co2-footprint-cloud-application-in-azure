using Application;
using Application.Services;
using Azure.Messaging.ServiceBus;
using InMemoryDatabase;
using MessageBusQueryWorker.Worker;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(MessageBusQueryFunction.Startup))]

namespace MessageBusQueryFunction;
public class Startup: FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddTransient<QueryProducts>();
        builder.Services.AddTransient<CalculateDiscount>();
        builder.Services.AddTransient<CalculateVatPrice>();
        builder.Services.AddSingleton<IReadonlyProductRepository, InMemoryReadonlyProductRepository>();
        var context = builder.GetContext();
        builder.Services.AddSingleton(new ServiceBusClient("SECRET")
        );
        builder.Services.AddSingleton(provider =>
            provider.GetRequiredService<ServiceBusClient>().CreateSender("worker-send-queue"));
        // builder.Services.Configure<QueueConfiguration>(context.Configuration.GetSection("ServiceBus")
        //     .GetSection("QueueConfiguration"));  
        // builder.Services.Configure<QueueConfiguration>(context.Configuration.GetSection("ServiceBus")
        //     .GetSection("QueueConfiguration"));
    }
}