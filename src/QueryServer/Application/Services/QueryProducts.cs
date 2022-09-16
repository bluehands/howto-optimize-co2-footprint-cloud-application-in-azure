namespace Application.Services;

public class QueryProducts
{
    private readonly IReadonlyProductRepository readonlyProductRepository;
    private readonly CalculateVatPrice calculateVatPrice;
    private readonly CalculateDiscount calculateDiscount;

    public QueryProducts(
        IReadonlyProductRepository readonlyProductRepository,
        CalculateVatPrice calculateVatPrice,
        CalculateDiscount calculateDiscount
    )
    {
        this.readonlyProductRepository = readonlyProductRepository;
        this.calculateVatPrice = calculateVatPrice;
        this.calculateDiscount = calculateDiscount;
    }

    public async Task<IEnumerable<Product>> OfCatalog()
    {
        return (await readonlyProductRepository
                .GetPageOfProducts(100))
            .Select(calculateDiscount.ForProduct)
            .Select(calculateVatPrice.OfProduct);
    }
}