using System.Text.Json;
using Application.Services;
using Azure.Messaging.ServiceBus;
using FunicularSwitch;
using MessagesContract;
using Microsoft.Extensions.Options;

namespace MessageBusQueryWorker.Worker;

public class QueryServiceWorker : BackgroundService
{
    private readonly ILogger<QueryServiceWorker> logger;
    private readonly IServiceScopeFactory scopeFactory;
    private readonly ServiceBusClient serviceBusClient;
    private readonly IOptions<QueueConfiguration> queueConfiguration;

    public QueryServiceWorker(
        ILogger<QueryServiceWorker> logger,
        IServiceScopeFactory scopeFactory,
        ServiceBusClient serviceBusClient,
        IOptions<QueueConfiguration> queueConfiguration)
    {
        this.logger = logger;
        this.scopeFactory = scopeFactory;
        this.serviceBusClient = serviceBusClient;
        this.queueConfiguration = queueConfiguration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Start Execution");
        // https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/servicebus/Azure.Messaging.ServiceBus/samples/Sample04_Processor.md
        // https://www.pluralsight.com/guides/how-to-use-managed-identity-with-azure-service-bus
        var options = new ServiceBusProcessorOptions
        {
            AutoCompleteMessages = false,
            MaxConcurrentCalls = 100,
            PrefetchCount = 100
        };
        
        await using var processor = serviceBusClient.CreateProcessor(
            queueConfiguration.Value.GatewaySendQueue,
            options);
        processor.ProcessMessageAsync += MessageHandler;
        processor.ProcessErrorAsync += ErrorHandler;
        logger.LogInformation("Begin processing messages");
        await processor.StartProcessingAsync(stoppingToken);
        await Task.Delay(10000000, stoppingToken);
        //
        // var rec = serviceBusClient.CreateReceiver(queueConfiguration.Value.GatewaySendQueue);
        // while (!stoppingToken.IsCancellationRequested)
        // {
        //     await foreach (var message in rec.ReceiveMessagesAsync(stoppingToken))
        //     {
        //         using var scope = scopeFactory.CreateScope();
        //         var queryProductsService = scope.ServiceProvider.GetRequiredService<QueryProducts>();
        //
        //         logger.LogInformation("RawMessage:{Message}", message.Body);
        //
        //         var queryProductsMessage = JsonSerializer
        //             .Deserialize<QueryProductsMessage>(message.Body)
        //             .ToOption();
        //
        //         var serviceBusSender = serviceBusClient.CreateSender(queueConfiguration.Value.WorkerSendQueue);
        //         await queryProductsMessage.Match(async productsMessage =>
        //         {
        //             logger.LogInformation("Request:CorrelationId:{ID}", productsMessage.CorrelationId);
        //
        //             var products = await queryProductsService.OfCatalog();
        //             var productItems = products
        //                 .Select(product => new ProductItem(
        //                     product.AmountAvailable,
        //                     product.EuroPrice,
        //                     product.ImageSource.GetValueOrDefault(),
        //                     product.ProductId,
        //                     product.ProductName))
        //                 .ToList();
        //
        //             var productMessage = new ProductsMessage(productsMessage.CorrelationId, productItems);
        //             var messageUtf8Bytes = JsonSerializer.SerializeToUtf8Bytes(productMessage);
        //             await serviceBusSender.SendMessageAsync(new ServiceBusMessage(messageUtf8Bytes), stoppingToken);
        //         });
        //
        //         // we can evaluate application logic and use that to determine how to settle the message.
        //         await rec.CompleteMessageAsync(message, stoppingToken);
        //     }
        // }
    }


    private async Task MessageHandler(ProcessMessageEventArgs args)
    {
        using var scope = scopeFactory.CreateScope();
        var queryProductsService = scope.ServiceProvider.GetRequiredService<QueryProducts>();

        logger.LogInformation("RawMessage:{Message}", args.Message.Body);

        var queryProductsMessage = JsonSerializer
            .Deserialize<QueryProductsMessage>(args.Message.Body)
            .ToOption();

        var serviceBusSender = serviceBusClient.CreateSender(queueConfiguration.Value.WorkerSendQueue);
        await queryProductsMessage.Match(async message =>
        {
            logger.LogInformation("Request:CorrelationId:{ID}", message.CorrelationId);

            var products = await queryProductsService.OfCatalog();
            var productItems = products
                .Select(product => new ProductItem(
                    product.AmountAvailable,
                    product.EuroPrice,
                    product.ImageSource.GetValueOrDefault(),
                    product.ProductId,
                    product.ProductName))
                .ToList();

            var productMessage = new ProductsMessage(message.CorrelationId, productItems);
            var messageUtf8Bytes = JsonSerializer.SerializeToUtf8Bytes(productMessage);
            await serviceBusSender.SendMessageAsync(new ServiceBusMessage(messageUtf8Bytes));
        });

        // we can evaluate application logic and use that to determine how to settle the message.
        await args.CompleteMessageAsync(args.Message);
    }

    private Task ErrorHandler(ProcessErrorEventArgs args)
    {
        // the error source tells me at what point in the processing an error occurred
        logger.LogError(args.ErrorSource.ToString());
        // the fully qualified namespace is available
        logger.LogError(args.FullyQualifiedNamespace);
        // as well as the entity path
        logger.LogError(args.EntityPath);
        logger.LogError(args.Exception.ToString());
        return Task.CompletedTask;
    }
}