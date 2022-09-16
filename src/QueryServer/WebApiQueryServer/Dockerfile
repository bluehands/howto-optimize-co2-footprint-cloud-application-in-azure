FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
#RUN dotnet dev-certs https
WORKDIR /src
COPY ["RestFulQueryServer/RestFulQueryServer.csproj", "RestFulQueryServer/"]
RUN dotnet restore "RestFulQueryServer/RestFulQueryServer.csproj"
COPY . .
WORKDIR "/src/RestFulQueryServer"
RUN dotnet build "RestFulQueryServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RestFulQueryServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
#COPY --from=publish /root/.dotnet/corefx/cryptography/x509stores/my/* /root/.dotnet/corefx/cryptography/x509stores/my/
#COPY --from=publish /app/publish .
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RestFulQueryServer.dll"]
