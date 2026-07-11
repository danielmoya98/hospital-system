using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace HospitalSystem.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Registra MediatR y le dice que busque los comandos/consultas en esta misma capa
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        return services;
    }
}