FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MessageBusGateway/MessageBusGateway.csproj", "MessageBusGateway/"]
RUN dotnet restore "MessageBusGateway/MessageBusGateway.csproj"
COPY . .
WORKDIR "/src/MessageBusGateway"
RUN dotnet build "MessageBusGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MessageBusGateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MessageBusGateway.dll"]
