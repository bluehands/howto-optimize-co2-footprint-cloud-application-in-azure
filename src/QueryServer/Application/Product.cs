using FunicularSwitch;

namespace Application;

public class Product
{
    public Guid ProductId { get; init; } 
    public string ProductName { get; init; } 
    
    public string ProductDescription { get; init; } 
    public Option<string> ImageSource { get;  init; } 
    public decimal EuroPrice { get;  init; }
    public int AmountAvailable { get;  init; }

    public Product UpdateEuroPrice(decimal newPrice)
    {
        return new Product
        {
            AmountAvailable = AmountAvailable,
            EuroPrice = newPrice,
            ImageSource = ImageSource,
            ProductId = ProductId,
            ProductName = ProductName,
        };
    }

    // public Product(string productId, string productName, string imageSource, decimal euroPrice, decimal oldEuroPrice, int amountAvailable)
    // {
    //     ProductId = productId;
    //     ProductName = productName;
    //     ImageSource = imageSource;
    //     EuroPrice = euroPrice;
    //     OldEuroPrice = oldEuroPrice;
    //     AmountAvailable = amountAvailable;
    // }
}