using System.Text.Json;
using Application;
using Azure.Messaging.ServiceBus;
using Contract;
using FunicularSwitch;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Options;

// ReSharper disable TemplateIsNotCompileTimeConstantProblem

namespace BasketCommandAppservice;

public class AddItemToBasketWorker : BackgroundService
{
    private readonly ILogger<AddItemToBasketWorker> logger;
    private readonly ServiceBusClient serviceBusClient;
    private readonly WorkerConfiguration workerConfiguration;
    private readonly TelemetryClient telemetryClient;
    private readonly StoreBasket storeBasket;

    public AddItemToBasketWorker(
        ILogger<AddItemToBasketWorker> logger,
        ServiceBusClient serviceBusClient,
        IOptions<WorkerConfiguration> workerConfiguration,
        TelemetryClient telemetryClient,
        StoreBasket storeBasket)
    {
        this.logger = logger;
        this.serviceBusClient = serviceBusClient;
        this.workerConfiguration = workerConfiguration.Value;
        this.telemetryClient = telemetryClient;
        this.storeBasket = storeBasket;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Worker running at: {Time}", DateTimeOffset.Now);
        var options = new ServiceBusProcessorOptions
        {
            AutoCompleteMessages = false,
            MaxConcurrentCalls = 10,
            PrefetchCount = 100
        };

        await using var processor = serviceBusClient.CreateProcessor(
            workerConfiguration.QueueName,
            options);
        processor.ProcessMessageAsync += MessageHandler;
        processor.ProcessErrorAsync += ErrorHandler;
        logger.LogInformation("Begin processing messages for {RunDuration} hours", workerConfiguration.RunDuration);
        await processor.StartProcessingAsync(stoppingToken);

        await Task.Delay(TimeSpan.FromHours(workerConfiguration.RunDuration), stoppingToken);
        logger.LogInformation("End processing");
        await telemetryClient.FlushAsync(CancellationToken.None);
        logger.LogInformation("Telemetry flushed");
    }

    private async Task MessageHandler(ProcessMessageEventArgs args)
    {
        // using var scope = scopeFactory.CreateScope();
        // var queryProductsService = scope.ServiceProvider.GetRequiredService<QueryProducts>();


        var basketAddedItemMessage = JsonSerializer
            .Deserialize<BasketAddedItemMessage>(args.Message.Body)
            .ToOption();


        var result = await basketAddedItemMessage.Match(async message =>
            {
                logger.LogInformation("Got Message {Corr}", message.CorrelationId);
                await storeBasket.Event(
                    new BasketAddedItem()
                    {
                        CorrelationId = message.CorrelationId,
                        Amount = message.Amount,
                        BasketId = message.BasketId,
                        ItemId = message.ItemId,
                    }
                );
                return Task.FromResult(Result.Ok(message));
            }, () =>
                Task.FromResult(Result.Error<BasketAddedItemMessage>("Got a message without content"))
        );

        await result.Match(
            async message =>
            {
                var now = DateTime.UtcNow;
                var duration = now - message.SentAt;
                telemetryClient.TrackMetric("DurationMs", duration.TotalMilliseconds);
                // telemetryClient.TrackEvent("AddItemToBasket",
                //     properties: new Dictionary<string, string>
                //     {
                //         { "CorrelationId", message.CorrelationId.ToString() },
                //         { "FinishedAtUtc", now.ToString("u") },
                //         { "SentAtUtc", message.SentAt.ToString("u") },
                //     },
                //     metrics: new Dictionary<string, double>
                //     {
                //         { "DurationInMilliseconds", duration.TotalMilliseconds }
                //     });
                await args.CompleteMessageAsync(args.Message);
            },
            error =>
            {
                telemetryClient.TrackException(new Exception(error));
                return Task.CompletedTask;
            });

        // we can evaluate application logic and use that to determine how to settle the message.
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

        telemetryClient.TrackException(args.Exception);
        return Task.CompletedTask;
    }
}