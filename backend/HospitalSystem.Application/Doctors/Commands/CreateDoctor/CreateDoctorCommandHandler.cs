using HospitalSystem.Application.Common.Interfaces;
using HospitalSystem.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.Application.Doctors.Commands.CreateDoctor;

public class CreateDoctorCommandHandler : IRequestHandler<CreateDoctorCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateDoctorCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
    {
        // 1. Validar que la especialidad exista
        var specialtyExists = await _context.Specialties.AnyAsync(s => s.Id == request.SpecialtyId, cancellationToken);
        if (!specialtyExists)
        {
            throw new Exception("La especialidad proporcionada no existe.");
        }

        // 2. Buscar o crear el Rol 'Doctor' para los permisos de seguridad
        var doctorRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Doctor", cancellationToken);
        if (doctorRole == null)
        {
            doctorRole = new Role 
            { 
                Name = "Doctor", 
                Description = "Personal Médico del Hospital",
                CreatedAt = DateTime.UtcNow // Prevenimos el error de null en la BD
            };
            _context.Roles.Add(doctorRole);
        }

        // 3. Crear el Usuario de Sistema
        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = request.Password, // En un entorno real de prod, aquí iría BCrypt
            Role = doctorRole,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        // 4. Crear el Perfil Médico ligado al Usuario y la Especialidad
        var doctor = new Doctor
        {
            User = user,
            FirstName = request.FirstName,
            LastName = request.LastName,
            LicenseNumber = request.LicenseNumber,
            PhoneNumber = request.PhoneNumber,
            SpecialtyId = request.SpecialtyId
        };

        // 5. Guardar todo en PostgreSQL
        _context.Doctors.Add(doctor);
        await _context.SaveChangesAsync(cancellationToken);

        return doctor.Id;
    }
}