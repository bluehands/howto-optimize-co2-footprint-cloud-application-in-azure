FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MessageBusQueryWorker/MessageBusQueryWorker.csproj", "MessageBusQueryWorker/"]
RUN dotnet restore "MessageBusQueryWorker/MessageBusQueryWorker.csproj"
COPY . .
WORKDIR "/src/MessageBusQueryWorker"
RUN dotnet build "MessageBusQueryWorker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MessageBusQueryWorker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MessageBusQueryWorker.dll"]
