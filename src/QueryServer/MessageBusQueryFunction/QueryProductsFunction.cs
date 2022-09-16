using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Services;
using Azure.Messaging.ServiceBus;
using FunicularSwitch;
using MessagesContract;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace MessageBusQueryFunction;

public class QueryProductsFunction
{
    private readonly QueryProducts queryProductsService;
    private readonly ServiceBusSender serviceBusSender;

    public QueryProductsFunction(
        QueryProducts queryProductsService, ServiceBusSender serviceBusSender)
    {
        this.queryProductsService = queryProductsService;
        this.serviceBusSender = serviceBusSender;
    }

    [FunctionName("QueryProductsFunction")]
    public async Task RunAsync([ServiceBusTrigger("gw-send-queue", Connection = "Connection")] string myQueueItem,
        ILogger log)
    {
        //  log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
        var queryProductsMessage = JsonSerializer
            .Deserialize<QueryProductsMessage>(myQueueItem)
            .ToOption();

        await queryProductsMessage.Match(async productsMessage =>
        {
            var products = await queryProductsService.OfCatalog();
            var productItems = products
                .Select(product => new ProductItem(
                    product.AmountAvailable,
                    product.EuroPrice,
                    product.ImageSource.GetValueOrDefault(),
                    product.ProductId,
                    product.ProductName))
                .ToList();

            var productMessage = new ProductsMessage(productsMessage.CorrelationId, productItems);
            var messageUtf8Bytes = JsonSerializer.SerializeToUtf8Bytes(productMessage);
            var serviceBusMessage = new ServiceBusMessage(messageUtf8Bytes);
            await serviceBusSender.SendMessageAsync(serviceBusMessage);
        });
    }
}