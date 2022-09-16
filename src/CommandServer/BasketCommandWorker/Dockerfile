FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BasketCommandWorker/BasketCommandWorker.csproj", "BasketCommandWorker/"]
RUN dotnet restore "BasketCommandWorker/BasketCommandWorker.csproj"
COPY . .
WORKDIR "/src/BasketCommandWorker"
RUN dotnet build "BasketCommandWorker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BasketCommandWorker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BasketCommandWorker.dll"]
