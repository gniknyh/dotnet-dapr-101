FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 7001

ENV ASPNETCORE_URLS=http://+:7001

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["src/mydapr.ApiService/mydapr.ApiService.csproj", "src/mydapr.ApiService/"]
RUN dotnet restore "src/mydapr.ApiService/mydapr.ApiService.csproj"
COPY . .
WORKDIR "/src/src/mydapr.ApiService"
RUN dotnet build "mydapr.ApiService.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "mydapr.ApiService.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "mydapr.ApiService.dll"]
