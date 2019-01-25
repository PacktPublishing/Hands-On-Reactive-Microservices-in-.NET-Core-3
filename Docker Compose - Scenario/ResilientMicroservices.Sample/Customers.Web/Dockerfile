FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY Customers.Web/Customers.Web.csproj Customers.Web/
RUN dotnet restore Customers.Web/Customers.Web.csproj
COPY . .
WORKDIR /src/Customers.Web
RUN dotnet build Customers.Web.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Customers.Web.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Customers.Web.dll"]
