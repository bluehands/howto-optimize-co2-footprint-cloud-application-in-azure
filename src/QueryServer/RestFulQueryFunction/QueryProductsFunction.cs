using System.Linq;
using System.Threading.Tasks;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RestFulContract;

namespace RestFulQueryFunction;

public class QueryProductsFunction
{
    private readonly QueryProducts queryProducts;

    public QueryProductsFunction(QueryProducts queryProducts)
    {
        this.queryProducts = queryProducts;
    }

    [FunctionName("QueryProducts")]
    public async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req, ILogger logger)
    {
        return new OkObjectResult(
            (await queryProducts.OfCatalog()).Select(
                product => new ProductItem(
                    product.AmountAvailable,
                    product.EuroPrice,
                    product.ImageSource.GetValueOrDefault(),
                    product.ProductId,
                    product.ProductName)
            )
        );
    }
}