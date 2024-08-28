FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TVR.Bundesliga.API/TVR.Bundesliga.API.csproj", "TVR.Bundesliga.API/"]
COPY ["TVR.Bundesliga.API.Core/TVR.Bundesliga.API.Core.csproj", "TVR.Bundesliga.API.Core/"]
COPY ["TVR.Bundesliga.API.Infrastructure/TVR.Bundesliga.API.Infrastructure.csproj", "TVR.Bundesliga.API.Infrastructure/"]
COPY ["TVR.Bundesliga.API.Domain/TVR.Bundesliga.API.Domain.csproj", "TVR.Bundesliga.API.Domain/"]
COPY ["TVR.Bundesliga.API.Contracts/TVR.Bundesliga.API.Contracts.csproj", "TVR.Bundesliga.API.Contracts/"]
RUN dotnet restore "TVR.Bundesliga.API/TVR.Bundesliga.API.csproj"
COPY . .
WORKDIR "/src/TVR.Bundesliga.API"
RUN dotnet build "TVR.Bundesliga.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TVR.Bundesliga.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TVR.Bundesliga.API.dll"]
