FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BasketCommandAppservice/BasketCommandAppservice.csproj", "BasketCommandAppservice/"]
RUN dotnet restore "BasketCommandAppservice/BasketCommandAppservice.csproj"
COPY . .
WORKDIR "/src/BasketCommandAppservice"
RUN dotnet build "BasketCommandAppservice.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BasketCommandAppservice.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BasketCommandAppservice.dll"]
