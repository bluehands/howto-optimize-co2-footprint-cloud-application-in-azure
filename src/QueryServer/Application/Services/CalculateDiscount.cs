namespace Application.Services;

public class CalculateDiscount
{
    private static decimal Discount => 0.1m;

    public Product ForProduct(Product product)
    {
        return product.UpdateEuroPrice(product.EuroPrice + product.EuroPrice * Discount);
    }
}