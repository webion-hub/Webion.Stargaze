FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Webion.Stargaze.Api/Webion.Stargaze.Api.csproj", "Webion.Stargaze.Api/"]
RUN dotnet restore "Webion.Stargaze.Api/Webion.Stargaze.Api.csproj"
COPY . .
WORKDIR "/src/Webion.Stargaze.Api"
RUN dotnet build "Webion.Stargaze.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Webion.Stargaze.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Webion.Stargaze.Api.dll"]
