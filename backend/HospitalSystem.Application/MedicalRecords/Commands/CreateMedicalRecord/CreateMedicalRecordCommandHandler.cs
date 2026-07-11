using HospitalSystem.Application.Common.Interfaces;
using HospitalSystem.Domain.Entities;
using HospitalSystem.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.Application.MedicalRecords.Commands.CreateMedicalRecord;

public class CreateMedicalRecordCommandHandler : IRequestHandler<CreateMedicalRecordCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateMedicalRecordCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateMedicalRecordCommand request, CancellationToken cancellationToken)
    {
        // 1. Buscar la cita original
        var appointment = await _context.Appointments
            .Include(a => a.Patient)
            .FirstOrDefaultAsync(a => a.Id == request.AppointmentId, cancellationToken);

        if (appointment == null) throw new Exception("La cita especificada no existe.");
        if (appointment.Status != AppointmentStatus.Scheduled) throw new Exception("Solo se pueden atender citas agendadas.");

        // 2. Crear el Historial
        var medicalRecord = new MedicalRecord
        {
            PatientId = appointment.PatientId,
            DoctorId = appointment.DoctorId,
            AppointmentId = appointment.Id,
            Diagnosis = request.Diagnosis,
            Treatment = request.Treatment,
            Notes = request.Notes
        };

        // 3. Cambiar el estado de la cita (Asumiendo que tienes un estado 'Completed' o similar en tu Enum)
        appointment.Status = AppointmentStatus.Completed; 

        _context.MedicalRecords.Add(medicalRecord);
        await _context.SaveChangesAsync(cancellationToken);

        return medicalRecord.Id;
    }
}