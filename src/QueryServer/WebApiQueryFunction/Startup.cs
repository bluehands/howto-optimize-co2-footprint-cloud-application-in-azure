using Application;
using Application.Services;
using Database;
using InMemoryDatabase;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using RestFulQueryFunction;

[assembly: FunctionsStartup(typeof(Startup))]

namespace RestFulQueryFunction;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddTransient<QueryProducts>();
        builder.Services.AddTransient<CalculateDiscount>();
        builder.Services.AddTransient<CalculateVatPrice>();
        builder.Services.AddSqlRepositories("SECRET");
    }
}