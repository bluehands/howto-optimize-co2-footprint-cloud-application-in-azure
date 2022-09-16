using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Application;
using Contract;
using FunicularSwitch;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace BasketCommandFunction
{
    public class Test
    {
        private readonly StoreBasket storeBasket;
        private readonly TelemetryClient telemetryClient;

        public Test(
            StoreBasket storeBasket,
            TelemetryConfiguration telemetryConfiguration
        )
        {
            this.storeBasket = storeBasket;
            this.telemetryClient = new TelemetryClient(telemetryConfiguration);
        }

        [FunctionName("AddItemToBasket")]
        public async Task RunAsync(
            [ServiceBusTrigger("min1", Connection = "ServiceBusConnection")]
            string myQueueItem, ILogger log)
        {
            log.LogInformation("Message");
            var basketAddedItemMessage = JsonSerializer
                .Deserialize<BasketAddedItemMessage>(myQueueItem)
                .ToOption();


            var result = await basketAddedItemMessage.Match(async message =>
                {
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

            await result.Match(message =>
                {
                    var now = DateTime.UtcNow;
                    var duration = now - message.SentAt;
                    telemetryClient.TrackMetric("DurationMs", duration.TotalMilliseconds);
                    return Task.CompletedTask;
                },
                error =>
                {
                    telemetryClient.TrackException(new Exception(error));
                    return Task.CompletedTask;
                }
            );
        }
    }
}