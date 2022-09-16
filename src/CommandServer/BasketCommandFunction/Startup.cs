using Application;
using AzureTableStorage;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(BasketCommandFunction.Startup))]

namespace BasketCommandFunction;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.RegisterAzureTableRepositories(
            "SECRET",
            "min1");
        builder.Services.AddTransient<StoreBasket>();
    }
}