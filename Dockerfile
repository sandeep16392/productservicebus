FROM mcr.microsoft.com/dotnet/core/sdk:3.0 as build AS base
WORKDIR /build

FROM mcr.microsoft.com/dotnet/core/sdk:3.0 as build AS build
WORKDIR /build

COPY ./ProductsQ.Api/ProductsQ.Api.csproj ./ProductsQ.Api/ProductsQ.Api.csproj
RUN dotnet restore ./ProductsQ.Api/ProductsQ.Api.csproj

COPY ./Products.ServiceBusMessaging/Products.ServiceBusMessaging.csproj ./Products.ServiceBusMessaging/Products.ServiceBusMessaging.csproj
RUN dotnet restore ./Products.ServiceBusMessaging/Products.ServiceBusMessaging.csproj

COPY ./Products.DAL/Products.DAL.csproj ./Products.DAL/Products.DAL.csproj
RUN dotnet restore ./Products.DAL/Products.DAL.csproj

COPY . .
RUN dotnet build "./ProductsQ.Api/ProductsQ.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./ProductsQ.Api/ProductsQ.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductsQ.Api.dll"]