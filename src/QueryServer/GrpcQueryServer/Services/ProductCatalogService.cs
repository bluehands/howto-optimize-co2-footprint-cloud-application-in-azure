using Application.Services;
using Grpc.Core;

namespace GrpcQueryServer.Services;

public class ProductCatalogService : ProductCatalog.ProductCatalogBase
{
    private readonly ILogger<ProductCatalogService> logger;
    private readonly QueryProducts queryProducts;

    public ProductCatalogService(ILogger<ProductCatalogService> logger, QueryProducts queryProducts)
    {
        this.logger = logger;
        this.queryProducts = queryProducts;
    }

    public override async Task<ProductsResponse> QueryProducts(ProductsRequest request, ServerCallContext context)
    {
        var products =
            (await queryProducts.OfCatalog())
            .Select(product => new ProductItem()
            {
                AmountAvailable = product.AmountAvailable,
                EuroPrice = product.EuroPrice,
                ImageSource = product.ImageSource.GetValueOrDefault(),
                ProductId = product.ProductId.ToString(),
                ProductName = product.ProductName
            });

        return new ProductsResponse()
        {
            Products = { products }
        };
    }
}