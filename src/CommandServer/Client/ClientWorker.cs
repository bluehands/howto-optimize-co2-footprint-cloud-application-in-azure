using System.Collections.Immutable;
using System.Diagnostics;
using System.Text.Json;
using Azure.Messaging.ServiceBus;
using ComposableAsync;
using Contract;
using Microsoft.Extensions.Options;
using RateLimiter;

namespace Client;

public class ClientWorker : BackgroundService
{
    private readonly ILogger<ClientWorker> logger;
    private readonly ServiceBusClient serviceBusClient;
    private readonly ClientConfiguration clientConfiguration;

    private readonly TimeLimiter timeConstraint;

    public ClientWorker(
        ILogger<ClientWorker> logger,
        ServiceBusClient serviceBusClient,
        IOptions<ClientConfiguration> clientConfiguration)
    {
        this.logger = logger;
        this.serviceBusClient = serviceBusClient;
        this.clientConfiguration = clientConfiguration.Value;
        timeConstraint = TimeLimiter.GetFromMaxCountByInterval(this.clientConfiguration.Iterations,
            TimeSpan.FromMilliseconds(this.clientConfiguration.PerMillisecond));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await RunQueries(stoppingToken);
    }

    private async Task RunQueries(CancellationToken stoppingToken)
    {
        var begin = DateTime.UtcNow;
        await File.AppendAllTextAsync(@"../begin.txt", begin.ToString("u") + Environment.NewLine, stoppingToken);
        var gatewaySender = serviceBusClient.CreateSender(clientConfiguration.QueueName);
        var end = begin + TimeSpan.FromHours(1);
        while (!stoppingToken.IsCancellationRequested && DateTime.UtcNow < end)
        {
            await timeConstraint;
            //var queryTimeStopwatch = Stopwatch.StartNew();
            var count = clientConfiguration.AmountOfMessages;
            var requests = Enumerable
                .Range(0, count)
                .Select(_ => CreateRequestMessage())
                .ToImmutableArray();

            await gatewaySender.SendMessagesAsync(
                requests.Select(CreateServiceBusMessage),
                stoppingToken);

            //queryTimeStopwatch.Stop();

            //logger.LogInformation("BatchTime:{Time},BatchSize:{Count}", queryTimeStopwatch.ElapsedMilliseconds, count);
        }
    }

    private BasketAddedItemMessage CreateRequestMessage()
    {
        return new BasketAddedItemMessage()
        {
            Amount = Random.Shared.Next(1, 100),
            BasketId = Guid.NewGuid(),
            CorrelationId = Guid.NewGuid(),
            ItemId = Guid.NewGuid(),
            SentAt = DateTime.UtcNow
        };
    }

    private ServiceBusMessage CreateServiceBusMessage(BasketAddedItemMessage productMessage)
    {
        var messageUtf8Bytes = JsonSerializer.SerializeToUtf8Bytes(productMessage);
        return new ServiceBusMessage(messageUtf8Bytes);
    }
}