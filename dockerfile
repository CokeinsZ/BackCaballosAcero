# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar y restaurar dependencias
COPY . .
WORKDIR /src/Api
RUN dotnet restore

# Construir
RUN dotnet publish Api.csproj -c Release -o /app

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app .

# Puerto de la aplicación
EXPOSE 8080
ENTRYPOINT ["dotnet", "Api.dll"]