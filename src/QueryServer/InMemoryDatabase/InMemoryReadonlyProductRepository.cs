using System.Collections.Immutable;
using Application;
using Bogus;

namespace InMemoryDatabase;

public class InMemoryReadonlyProductRepository : IReadonlyProductRepository
{
    // ReSharper disable once CollectionNeverUpdated.Local
    private readonly Dictionary<Guid, Product> products = new();

    public InMemoryReadonlyProductRepository()
    {
        FakeProducts()
            .ForEach(product => products.Add(product.ProductId, product));
    }

    private static List<Product> FakeProducts()
    {
        var fakeProducts = new List<Product>();
        var faker = new Faker("de");
        for (int i = 0; i < 1000; i++)
        {
            fakeProducts.Add(
                new Product
                {
                    AmountAvailable = faker.Random.Int(0, 10000),
                    EuroPrice = faker.Finance.Amount(0, 4000),
                    ImageSource = faker.Image.LoremPixelUrl(),
                    ProductId = Guid.NewGuid(),
                    ProductName = faker.Commerce.ProductName(),
                    ProductDescription = faker.Commerce.ProductDescription()
                });
        }

        return fakeProducts;
    }

    public Task<IReadOnlyCollection<Product>> GetPageOfProducts(int pageSize)
    {
        IReadOnlyCollection<Product> immutableArray = products.Values.Take(pageSize).ToImmutableArray();
        return Task.FromResult(immutableArray);
    }
}