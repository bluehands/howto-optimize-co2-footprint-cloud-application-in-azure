using Application.Services;
using Microsoft.AspNetCore.Mvc;
using RestFulContract;

namespace WebApiQueryServer.Controllers;

[ApiController]
[Route("[controller]")]
public class QueryProductsController : ControllerBase
{
    private readonly ILogger<QueryProductsController> logger;
    private readonly QueryProducts queryProducts;

    public QueryProductsController(ILogger<QueryProductsController> logger, QueryProducts queryProducts)
    {
        this.logger = logger;
        this.queryProducts = queryProducts;
    }

    [HttpGet(Name = "QueryProducts")]
    public async Task<IActionResult> Get()
    {
        return new OkObjectResult(
            (await queryProducts.OfCatalog()).Select(
                product => new ProductItem(
                    product.AmountAvailable,
                    product.EuroPrice,
                    product.ImageSource.FirstOrDefault(),
                    product.ProductId,
                    product.ProductName)
            )
        );
    }

}