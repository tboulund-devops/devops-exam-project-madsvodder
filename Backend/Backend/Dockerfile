# Sørg for at versionen (10.0) matcher jeres csproj fil
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# RETTELSE: Stien skal være Backend/Backend/ (da der er en mappe i en mappe)
COPY ["Backend/Backend/Backend.csproj", "Backend/Backend/"]
RUN dotnet restore "Backend/Backend/Backend.csproj"

# Kopier alt ind
COPY . .

# RETTELSE: Flyt til den korrekte undermappe før build
WORKDIR "/src/Backend/Backend"
RUN dotnet build "./Backend.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Backend.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Backend.dll"]