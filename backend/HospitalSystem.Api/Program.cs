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

// Agregar servicios al contenedor.
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

// 1. Configurar Swagger para que acepte Tokens JWT (A prueba de colisiones)
        builder.Services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Description = "Ingresa el token JWT de esta manera: Bearer {tu_token}",
                Name = "Authorization",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
            {
                {
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Reference = new Microsoft.OpenApi.Models.OpenApiReference
                        {
                            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
// Inyectar nuestras capas de Clean Architecture
        builder.Services.AddInfrastructureServices(builder.Configuration);
        builder.Services.AddApplicationServices();

// 2. Configurar la Autenticación (Cómo validar el token que llega)
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                    ValidAudience = builder.Configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"]!))
                };
            });

        builder.Services.AddAuthorization();

        builder.Services.AddMassTransit(x =>
        {
            x.AddConsumer<PatientRegisteredEventConsumer>(); // <-- 1. Registramos el Consumer
            x.AddConsumer<LabResultProcessedEventConsumer>();
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ConfigureEndpoints(context); // <-- 2. Auto-configura las colas e intercambios
            });
        });
        
        var app = builder.Build();

// Configurar el pipeline HTTP.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

// 3. ¡El orden es vital! Primero Autenticar (Quién eres), luego Autorizar (Qué puedes hacer)
        app.UseAuthentication(); 
        app.UseAuthorization();

        app.MapControllers();
        app.Run();
    }
}