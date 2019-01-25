FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY Orders.Web/Orders.Web.csproj Orders.Web/
RUN dotnet restore Orders.Web/Orders.Web.csproj
COPY . .
WORKDIR /src/Orders.Web
RUN dotnet build Orders.Web.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Orders.Web.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Orders.Web.dll"]
