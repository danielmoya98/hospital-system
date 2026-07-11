using HospitalSystem.Application.Common.Interfaces;
using HospitalSystem.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.Application.Auth.Commands.Bootstrap;

public class BootstrapCommandHandler : IRequestHandler<BootstrapCommand, string>
{
    private readonly IApplicationDbContext _context;

    public BootstrapCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<string> Handle(BootstrapCommand request, CancellationToken cancellationToken)
    {
        // 1. Verificación de seguridad: Evitar doble ejecución
        var adminExists = await _context.Users.AnyAsync(u => u.Email == "admin@hospitalsystem.com", cancellationToken);
        if (adminExists)
        {
            throw new InvalidOperationException("El sistema ya ha sido inicializado previamente.");
        }

        // 2. Crear el Rol de Administrador
        var adminRole = new Role 
        { 
            Name = "Admin", 
            Description = "Administrador Global del Sistema" 
        };
        _context.Roles.Add(adminRole);
        await _context.SaveChangesAsync(cancellationToken);

        // 3. Crear el Usuario Maestro (Texto plano por ahora, como exige tu LoginCommandHandler)
        var masterUser = new User
        {
            Username = "admin.maestro",
            Email = "admin@hospitalsystem.com",
            PasswordHash = "HospitalAdmin2026!", // Texto plano temporal
            IsActive = true,
            RoleId = adminRole.Id
        };

        _context.Users.Add(masterUser);
        await _context.SaveChangesAsync(cancellationToken);

        return "Sistema inicializado con éxito. Email: admin@hospitalsystem.com / Pass: HospitalAdmin2026!";
    }
}