using HospitalSystem.Application.Common.Interfaces;
using HospitalSystem.Domain.Enums;
using HospitalSystem.Infrastructure.Auth;
using HospitalSystem.Infrastructure.Data;
using HospitalSystem.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;

namespace HospitalSystem.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // 1. Configuración de Base de Datos (PostgreSQL / Neon)
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString, npgsqlOptions => 
            {
                // Mapeo necesario en versiones recientes de Npgsql para usar Enums a nivel de base de datos
                npgsqlOptions.MapEnum<AppointmentStatus>("appointment_status");
                npgsqlOptions.MapEnum<GenderType>("gender_type");
                npgsqlOptions.MapEnum<BloodType>("blood_type");
            });
        });
        
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        
        // 2. Configuración de Seguridad (JWT)
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        // 3. Configuración de Almacenamiento de Archivos (Storj / S3 API)
        services.AddMinio(configureClient =>
        {
            // Lectura segura de variables con valores por defecto para evitar caídas del servidor
            var endpoint = configuration["MinioSettings:Endpoint"] ?? "localhost:9000";
            var accessKey = configuration["MinioSettings:AccessKey"] ?? "minioadmin";
            var secretKey = configuration["MinioSettings:SecretKey"] ?? "minioadmin";
            var withSSL = bool.Parse(configuration["MinioSettings:WithSSL"] ?? "false");

            configureClient
                .WithEndpoint(endpoint)
                .WithCredentials(accessKey, secretKey);

            // Activación condicional del cifrado (obligatorio para la nube, opcional en local)
            if (withSSL)
            {
                configureClient.WithSSL();
            }
        });

        services.AddScoped<IFileStorageService, MinioFileStorageService>();

        return services;
    }
}