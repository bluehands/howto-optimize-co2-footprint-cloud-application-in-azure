namespace Application.Services;

public class CalculateVatPrice
{
    private static decimal Vat => 0.19m;

    public Product OfProduct(Product netPricedProduct)
    {
        return netPricedProduct.UpdateEuroPrice(netPricedProduct.EuroPrice + netPricedProduct.EuroPrice * Vat);
    }
}