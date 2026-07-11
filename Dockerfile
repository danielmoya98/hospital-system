# Etapa 1: Construcción
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copiar archivos de proyecto y restaurar dependencias
COPY ["backend/HospitalSystem.Api/HospitalSystem.Api.csproj", "backend/HospitalSystem.Api/"]
COPY ["backend/HospitalSystem.Application/HospitalSystem.Application.csproj", "backend/HospitalSystem.Application/"]
COPY ["backend/HospitalSystem.Domain/HospitalSystem.Domain.csproj", "backend/HospitalSystem.Domain/"]
COPY ["backend/HospitalSystem.Infrastructure/HospitalSystem.Infrastructure.csproj", "backend/HospitalSystem.Infrastructure/"]
RUN dotnet restore "backend/HospitalSystem.Api/HospitalSystem.Api.csproj"

# Copiar el resto del código y compilar
COPY backend/ backend/
WORKDIR "/src/backend/HospitalSystem.Api"
RUN dotnet build "HospitalSystem.Api.csproj" -c Release -o /app/build
RUN dotnet publish "HospitalSystem.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa 2: Ejecución
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Exponer el puerto dinámico de Render
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000

ENTRYPOINT ["dotnet", "HospitalSystem.Api.dll"]
# Etapa 1: Construcción
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copiar archivos de proyecto y restaurar dependencias
COPY ["backend/HospitalSystem.Api/HospitalSystem.Api.csproj", "backend/HospitalSystem.Api/"]
COPY ["backend/HospitalSystem.Application/HospitalSystem.Application.csproj", "backend/HospitalSystem.Application/"]
COPY ["backend/HospitalSystem.Domain/HospitalSystem.Domain.csproj", "backend/HospitalSystem.Domain/"]
COPY ["backend/HospitalSystem.Infrastructure/HospitalSystem.Infrastructure.csproj", "backend/HospitalSystem.Infrastructure/"]
RUN dotnet restore "backend/HospitalSystem.Api/HospitalSystem.Api.csproj"

# Copiar el resto del código y compilar
COPY backend/ backend/
WORKDIR "/src/backend/HospitalSystem.Api"
RUN dotnet build "HospitalSystem.Api.csproj" -c Release -o /app/build
RUN dotnet publish "HospitalSystem.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa 2: Ejecución
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Exponer el puerto dinámico de Render
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000

ENTRYPOINT ["dotnet", "HospitalSystem.Api.dll"]
