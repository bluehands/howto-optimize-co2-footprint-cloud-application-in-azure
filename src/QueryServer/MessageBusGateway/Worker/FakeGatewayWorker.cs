using System.Diagnostics;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text.Json;
using Azure.Messaging.ServiceBus;
using MessagesContract;
using Microsoft.Extensions.Options;

namespace MessageBusGateway.Worker;

public class FakeGatewayWorker : BackgroundService
{
    private readonly ILogger<FakeGatewayWorker> logger;
    private readonly Subject<ProductsMessage> productsMessageSubject;
    private readonly ServiceBusClient serviceBusClient;
    private readonly IOptions<QueueConfiguration> queueConfiguration;

    public FakeGatewayWorker(
        Subject<ProductsMessage> productsMessageSubject,
        ServiceBusClient serviceBusClient,
        ILogger<FakeGatewayWorker> logger, IOptions<QueueConfiguration> queueConfiguration)
    {
        this.productsMessageSubject = productsMessageSubject;
        this.serviceBusClient = serviceBusClient;
        this.logger = logger;
        this.queueConfiguration = queueConfiguration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Start execution");

        await RunQueries(stoppingToken);
    }

    private async Task RunQueries(CancellationToken stoppingToken)
    {
        var gatewaySender = serviceBusClient.CreateSender(queueConfiguration.Value.GatewaySendQueue);
        while (!stoppingToken.IsCancellationRequested)
        {
            var queryTimeStopwatch = Stopwatch.StartNew();


            var count = 1000;
            var requests = Enumerable.Range(1, count).Select(_ => CreateRequestMessage()).ToList();

            await gatewaySender.SendMessagesAsync(
                requests.Select(CreateServiceBusMessage),
                stoppingToken);

            var response = requests.Select(async message =>
                await productsMessageSubject.FirstAsync(e => e.CorrelationId == message.CorrelationId));
            
            await Task.WhenAll(response);
            queryTimeStopwatch.Stop();

            logger.LogInformation("BatchTime:{Time},BatchSize:{Count}", queryTimeStopwatch.ElapsedMilliseconds, count);
            // logger.LogInformation(
            //     "Response:CorrelationId:{CorrelationId},Duration:{Count}",
            //     response.CorrelationId,
            //     queryTimeStopwatch.ElapsedMilliseconds);
        }
    }

    private QueryProductsMessage CreateRequestMessage()
    {
        var requestCorrelationId = Guid.NewGuid();
        return new QueryProductsMessage(requestCorrelationId);
    }

    private ServiceBusMessage CreateServiceBusMessage(QueryProductsMessage productMessage)
    {
        var messageUtf8Bytes = JsonSerializer.SerializeToUtf8Bytes(productMessage);
        return new ServiceBusMessage(messageUtf8Bytes);
    }
}