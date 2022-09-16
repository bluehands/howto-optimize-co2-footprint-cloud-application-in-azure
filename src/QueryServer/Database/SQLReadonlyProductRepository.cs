using System.Collections.Immutable;
using Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Database;

public class SqlReadonlyProductRepository : IReadonlyProductRepository
{
    private readonly ILogger<SqlReadonlyProductRepository> logger;
    private readonly ProductContext productContext;

    public SqlReadonlyProductRepository(ILogger<SqlReadonlyProductRepository> logger,ProductContext productContext)
    {
        this.logger = logger;
        this.productContext = productContext;
    }

    public async Task<IReadOnlyCollection<Product>> GetPageOfProducts(int pageSize)
    {
        logger.LogInformation("DB Access");
        return (await productContext.Products.Take(pageSize).ToListAsync()).ToImmutableArray();
    }
}