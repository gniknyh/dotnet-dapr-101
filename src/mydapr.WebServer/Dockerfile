FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80 443

ENV ASPNETCORE_URLS=http://+:80

COPY ["cert/link_mkdcert_rootCA.crt", "/usr/local/share/ca-certificates/link_mkdcert_rootCA.crt"]
RUN chmod 644 /usr/local/share/ca-certificates/link_mkdcert_rootCA.crt && update-ca-certificates


USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["src/mydapr.WebServer/mydapr.WebServer.csproj", "src/mydapr.WebServer/"]
RUN dotnet restore "src/mydapr.WebServer/mydapr.WebServer.csproj"
COPY . .
WORKDIR "/src/src/mydapr.WebServer"
RUN dotnet build "mydapr.WebServer.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "mydapr.WebServer.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "mydapr.WebServer.dll"]
