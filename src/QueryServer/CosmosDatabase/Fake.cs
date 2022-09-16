using Application;
using Bogus;

namespace CosmosDatabase;

public class Fake
{
    private static readonly Faker Faker = new Faker("de");

    public static Product FakeProduct()
    {
        return new Product
        {
            AmountAvailable = Faker.Random.Int(0, 10000),
            EuroPrice = Faker.Finance.Amount(0, 4000),
            ImageSource = Faker.Image.LoremPixelUrl(),
            ProductId = Guid.NewGuid(),
            ProductName = Faker.Commerce.ProductName(),
            ProductDescription = Faker.Commerce.ProductDescription()
        };
    }
}