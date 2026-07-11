using System.Text;
using HospitalSystem.Api.Consumers;
using HospitalSystem.Application;
using HospitalSystem.Infrastructure;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

public partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Asignación de puerto dinámico para el contenedor de Render
        var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
        builder.WebHost.UseUrls($"http://*:{port}");

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        // Configuración de CORS adaptada para consumir la API desde Angular local
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            {
                var frontendUrl = builder.Configuration["FrontendUrl"] ?? "http://localhost:4200";
                policy.WithOrigins(frontendUrl)
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
            });
        });

        builder.Services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Ingresa el token JWT: Bearer {tu_token}",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    Array.Empty<string>()
                }
            });
        });

        // Inyección de dependencias de las capas de Clean Architecture
        builder.Services.AddInfrastructureServices(builder.Configuration);
        builder.Services.AddApplicationServices();

        // Validación del esquema de autenticación JWT
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["JwtSettings:Issuer"] ?? "HospitalSystemApi",
                    ValidAudience = builder.Configuration["JwtSettings:Audience"] ?? "HospitalSystemFrontend",
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"] ?? "HospitalSystem_Super_Secret_Key_For_JWT_2026_Min_32_Chars!"))
                };
            });

        builder.Services.AddAuthorization();

        // Configuración de mensajería asíncrona con MassTransit y CloudAMQP
        builder.Services.AddMassTransit(x =>
        {
            x.AddConsumer<PatientRegisteredEventConsumer>();
            x.AddConsumer<LabResultProcessedEventConsumer>();
            
            x.UsingRabbitMq((context, cfg) =>
            {
                var rmqHost = builder.Configuration["RabbitMq:Host"] ?? "localhost";
                var rmqVHost = builder.Configuration["RabbitMq:VHost"] ?? "/";
                var rmqUser = builder.Configuration["RabbitMq:Username"] ?? "guest";
                var rmqPass = builder.Configuration["RabbitMq:Password"] ?? "guest";

                // Definimos el puerto: 5671 para la nube (SSL) y 5672 para tu Docker local
                ushort rmqPort = (ushort)(rmqHost == "localhost" ? 5672 : 5671);

                // Pasamos el puerto explícitamente a la configuración del Host
                cfg.Host(rmqHost, rmqPort, rmqVHost, h =>
                {
                    h.Username(rmqUser);
                    h.Password(rmqPass);
                    
                    if (rmqHost != "localhost")
                    {
                        // Ahora sí, el SSL negociará correctamente a través del puerto 5671
                        h.UseSsl(s => { s.Protocol = System.Security.Authentication.SslProtocols.Tls12; });
                    }
                });

                cfg.ConfigureEndpoints(context);
            });
        });
        
        var app = builder.Build();

        // Swagger expuesto públicamente en Render para pruebas de integración iniciales
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        // El middleware de CORS debe ejecutarse estrictamente antes de la autenticación
        app.UseCors("AllowFrontend");

        app.UseAuthentication(); 
        app.UseAuthorization();

        app.MapControllers();
        app.Run();
    }
}