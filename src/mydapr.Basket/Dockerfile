FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

ENV ASPNETCORE_URLS=http://+:80

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["src/mydapr.Basket/mydapr.Basket.csproj", "src/mydapr.Basket/"]
RUN dotnet restore "src/mydapr.Basket/mydapr.Basket.csproj"
COPY . .
WORKDIR "/src/src/mydapr.Basket"
RUN dotnet build "mydapr.Basket.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "mydapr.Basket.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "mydapr.Basket.dll"]
