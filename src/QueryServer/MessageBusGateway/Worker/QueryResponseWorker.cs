using System.Reactive.Subjects;
using System.Text.Json;
using Azure.Messaging.ServiceBus;
using FunicularSwitch;
using MessagesContract;
using Microsoft.Extensions.Options;

namespace MessageBusGateway.Worker;

public class QueryResponseWorker : BackgroundService
{
    private readonly ILogger<QueryResponseWorker> logger;

    private readonly ServiceBusClient serviceBusClient;
    private readonly Subject<ProductsMessage> productsMessageSubject;
    private readonly IOptions<QueueConfiguration> queueConfiguration;

    public QueryResponseWorker(
        ILogger<QueryResponseWorker> logger,
        ServiceBusClient serviceBusClient,
        Subject<ProductsMessage> productsMessageSubject,
        IOptions<QueueConfiguration> queueConfiguration)
    {
        this.logger = logger;
        this.serviceBusClient = serviceBusClient;
        this.productsMessageSubject = productsMessageSubject;
        this.queueConfiguration = queueConfiguration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        // var rec = serviceBusClient.CreateReceiver(queueConfiguration.Value.WorkerSendQueue);
        // while (!stoppingToken.IsCancellationRequested)
        // {
        //     await foreach (var message in rec.ReceiveMessagesAsync(stoppingToken))
        //     {
        //         var queryProductsMessage = JsonSerializer
        //             .Deserialize<ProductsMessage>(message.Body)
        //             .ToOption();
        //
        //         queryProductsMessage.Match(productsMessage =>
        //         {
        //             productsMessageSubject.OnNext(productsMessage);
        //         });
        //
        //         // we can evaluate application logic and use that to determine how to settle the message.
        //         await rec.CompleteMessageAsync(message, stoppingToken);
        //     }
        // }
        logger.LogInformation("Start execution");
        // https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/servicebus/Azure.Messaging.ServiceBus/samples/Sample04_Processor.md
        // https://www.pluralsight.com/guides/how-to-use-managed-identity-with-azure-service-bus
        var options = new ServiceBusProcessorOptions
        {
            AutoCompleteMessages = false,
            MaxConcurrentCalls = 100,
            PrefetchCount = 100
        };
        
        await using ServiceBusProcessor processor =
            serviceBusClient.CreateProcessor(queueConfiguration.Value.WorkerSendQueue, options);
        processor.ProcessMessageAsync += MessageHandler;
        processor.ProcessErrorAsync += ErrorHandler;
        logger.LogInformation("Worker processor ready");
        await processor.StartProcessingAsync(stoppingToken);
        await Task.Delay(10000000, stoppingToken);
    }

    private async Task MessageHandler(ProcessMessageEventArgs args)
    {
        var queryProductsMessage = JsonSerializer
            .Deserialize<ProductsMessage>(args.Message.Body)
            .ToOption();

        queryProductsMessage.Match(message =>
        {
            productsMessageSubject.OnNext(message);
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