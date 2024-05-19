FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

ENV ASPNETCORE_ENVIRONMENT=Development

USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["product.csproj", "product/"]
RUN dotnet restore "product/product.csproj"
WORKDIR "/src/product/"
COPY . .
RUN dotnet build "product.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "product.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "product.dll"]
