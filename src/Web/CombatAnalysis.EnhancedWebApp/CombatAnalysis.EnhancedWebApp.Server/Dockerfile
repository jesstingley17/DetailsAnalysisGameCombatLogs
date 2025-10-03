# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Web/CombatAnalysis.EnhancedWebApp/CombatAnalysis.EnhancedWebApp.Server/CombatAnalysis.EnhancedWebApp.Server.csproj", "src/Web/CombatAnalysis.EnhancedWebApp/CombatAnalysis.EnhancedWebApp.Server/"]
RUN dotnet restore "./src/Web/CombatAnalysis.EnhancedWebApp/CombatAnalysis.EnhancedWebApp.Server/CombatAnalysis.EnhancedWebApp.Server.csproj"
COPY . .
WORKDIR "/src/src/Web/CombatAnalysis.EnhancedWebApp/CombatAnalysis.EnhancedWebApp.Server"
RUN dotnet build "./CombatAnalysis.EnhancedWebApp.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build


FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CombatAnalysis.EnhancedWebApp.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CombatAnalysis.EnhancedWebApp.Server.dll"]
