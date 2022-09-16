using Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Database;

public static class RegisterSqlDb
{
    public static IServiceCollection AddSqlRepositories(
        this IServiceCollection serviceCollection,
        string connectionString)
    {
        serviceCollection.AddDbContext<ProductContext>(optionsBuilder =>
        {
            optionsBuilder.UseSqlServer(connectionString);
        });
        serviceCollection.AddScoped<IReadonlyProductRepository, SqlReadonlyProductRepository>();
        return serviceCollection;
    }
}