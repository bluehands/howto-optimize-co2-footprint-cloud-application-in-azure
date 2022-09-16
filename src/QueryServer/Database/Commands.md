dotnet ef migrations add InitialCreate --project Database --startup-project GrpcQueryServer
dotnet ef migrations remove  --project Database --startup-project GrpcQueryServer

dotnet ef database update --project Database --startup-project RestFulQueryServer
dotnet ef migrations add InitialCreate --project Database --startup-project RestFulQueryServer
dotnet ef migrations remove  --project Database --startup-project RestFulQueryServer




