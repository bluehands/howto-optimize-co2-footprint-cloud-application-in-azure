using System.Collections.Immutable;

namespace Application;

public interface IReadonlyProductRepository
{
    public Task<IReadOnlyCollection<Product>> GetPageOfProducts(int pageSize);
}