using Grpc.Net.Client;
using GrpcQueryServer;
using NBomber.Contracts;
using NBomber.CSharp;

namespace GrpcQueryClient;

public class GrpcClientFactory : IClientFactory<ProductCatalog.ProductCatalogClient>
{
    
    private readonly GrpcChannel grpcChannel;
    
    public GrpcClientFactory(string factoryName, GrpcChannel grpcChannel)
    {
        FactoryName = factoryName;
        this.grpcChannel = grpcChannel;
    }

    public Task<ProductCatalog.ProductCatalogClient> InitClient(int number, IBaseContext context)
    {
    
        return Task.FromResult(new ProductCatalog.ProductCatalogClient(grpcChannel));
    }
    
    public Task DisposeClient(ProductCatalog.ProductCatalogClient client, IBaseContext context)
    {
    
        return Task.CompletedTask;
        
    }
    
    public string FactoryName { get; }
    public int ClientCount { get; }
}