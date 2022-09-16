FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["GrpcQueryClient/GrpcQueryClient.csproj", "GrpcQueryClient/"]
RUN dotnet restore "GrpcQueryClient/GrpcQueryClient.csproj"
COPY . .
WORKDIR "/src/GrpcQueryClient"
RUN dotnet build "GrpcQueryClient.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GrpcQueryClient.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GrpcQueryClient.dll"]
