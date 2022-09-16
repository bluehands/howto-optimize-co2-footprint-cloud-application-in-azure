// See https://aka.ms/new-console-template for more information

using CosmosDatabase;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SeedCosmosDb;

Console.WriteLine("Hello, World!");


var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddCosmosRepositories();
        services.AddHostedService<SeedWorker>();
        services.AddLogging(builder => builder.AddConsole());
    })
    .Build();


await host.RunAsync();