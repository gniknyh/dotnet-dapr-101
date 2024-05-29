FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80 443

ENV ASPNETCORE_URLS=http://+:80

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["src/mydapr.Identity/mydapr.Identity.csproj", "src/mydapr.Identity/"]
RUN dotnet restore "src/mydapr.Identity/mydapr.Identity.csproj"
COPY . .
WORKDIR "/src/src/mydapr.Identity"
RUN dotnet build "mydapr.Identity.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "mydapr.Identity.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "mydapr.Identity.dll"]
