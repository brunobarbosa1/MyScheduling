# ==========================================================
# Dockerfile da API (MyScheduling.API)
# Contexto de build esperado: RAIZ do repositório.
#   docker build -f UI/MyScheduling.API/Dockerfile .
# (o docker-compose já configura o contexto corretamente)
# ==========================================================

# ---------- Build ----------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copia apenas os .csproj primeiro para aproveitar cache de restore
COPY NuGet.config ./
COPY Core/MyScheduling.Common/MyScheduling.Common.csproj Core/MyScheduling.Common/
COPY Core/MyScheduling.DependencyInjection/MyScheduling.DependencyInjection.csproj Core/MyScheduling.DependencyInjection/
COPY Modules/MyScheduling.Domain/MyScheduling.Domain.csproj Modules/MyScheduling.Domain/
COPY Modules/MyScheduling.Data/MyScheduling.Data.csproj Modules/MyScheduling.Data/
COPY Modules/MyScheduling.Application/MyScheduling.Application.csproj Modules/MyScheduling.Application/
COPY Modules/MyScheduling.Presentations/MyScheduling.Presentations.csproj Modules/MyScheduling.Presentations/
COPY UI/MyScheduling.API/MyScheduling.API.csproj UI/MyScheduling.API/
RUN dotnet restore UI/MyScheduling.API/MyScheduling.API.csproj

# Copia o restante do código e publica
COPY . .
RUN dotnet publish UI/MyScheduling.API/MyScheduling.API.csproj \
    -c Release -o /app/publish /p:UseAppHost=false

# ---------- Runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "MyScheduling.API.dll"]
