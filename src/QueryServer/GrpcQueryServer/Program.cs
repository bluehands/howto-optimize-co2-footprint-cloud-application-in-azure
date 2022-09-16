using Application.Services;
using Database;
using GrpcQueryServer.Services;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddTransient<QueryProducts>();
builder.Services.AddTransient<CalculateDiscount>();
builder.Services.AddTransient<CalculateVatPrice>();
builder.Services.AddSqlRepositories(builder.Configuration.GetConnectionString("db"));

builder.Services.AddGrpc();
builder.Services.AddLogging();
builder.Services.AddApplicationInsightsTelemetry();

builder.WebHost.ConfigureKestrel(options => 
{ 
    options.ListenAnyIP(8080); 
    options.ListenAnyIP(8585, listenOptions => 
    { 
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2; 
    }); 
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<ProductCatalogService>();

app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();