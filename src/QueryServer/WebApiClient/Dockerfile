FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["RestFulQueryClient/RestFulQueryClient.csproj", "RestFulQueryClient/"]
RUN dotnet restore "RestFulQueryClient/RestFulQueryClient.csproj"
COPY . .
WORKDIR "/src/RestFulQueryClient"
RUN dotnet build "RestFulQueryClient.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RestFulQueryClient.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RestFulQueryClient.dll"]
