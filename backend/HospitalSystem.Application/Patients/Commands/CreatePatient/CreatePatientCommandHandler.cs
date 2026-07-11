using HospitalSystem.Application.Common.Interfaces;
using HospitalSystem.Application.Common.Events; // <-- Traemos el record del evento
using HospitalSystem.Domain.Entities;
using MassTransit; 
using MediatR;

namespace HospitalSystem.Application.Patients.Commands.CreatePatient;

public class CreatePatientCommandHandler : IRequestHandler<CreatePatientCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IPublishEndpoint _publishEndpoint; // <-- Inyectamos la abstracción del Bus

    // El constructor ahora recibe tanto el contexto de base de datos como el bus de eventos
    public CreatePatientCommandHandler(IApplicationDbContext context, IPublishEndpoint publishEndpoint)
    {
        _context = context;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<Guid> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
    {
        // 1. Mapear el comando a la Entidad de Dominio
        var patient = new Patient
        {
            NationalId = request.NationalId,
            FirstName = request.FirstName,
            LastName = request.LastName,
            DateOfBirth = request.DateOfBirth,
            Gender = request.Gender,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email
        };

        // 2. Guardar en PostgreSQL a través de la abstracción transaccional
        _context.Patients.Add(patient);
        await _context.SaveChangesAsync(cancellationToken);

        // 3. Publicar el evento asíncrono en RabbitMQ
        // Esto se envía de forma inmediata al exchange correspondiente
        await _publishEndpoint.Publish(new PatientRegisteredEvent(
            patient.Id,
            patient.NationalId,
            $"{patient.FirstName} {patient.LastName}",
            patient.Email
        ), cancellationToken);

        // 4. Retornar el ID generado
        return patient.Id;
    }
}