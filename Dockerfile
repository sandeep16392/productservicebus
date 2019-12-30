#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /build

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
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