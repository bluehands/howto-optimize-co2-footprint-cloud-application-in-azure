using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace BasketFunction;

public static class ProcessBasketAddedItemFunction
{
    [FunctionName("ProcessBasketAddedItemFunction")]
    public static async Task RunAsync([ServiceBusTrigger("low-queue", Connection = "ServiceBusConnection")] string myQueueItem, ILogger log)
    {
        log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
        
    }
}