using HospitalSystem.Application.Common.Interfaces;
using HospitalSystem.Domain.Entities;
using HospitalSystem.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.Application.Appointments.Commands.CreateAppointment;

public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateAppointmentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
    {
        // 1. Validar que el Paciente exista
        var patientExists = await _context.Patients.AnyAsync(p => p.Id == request.PatientId, cancellationToken);
        if (!patientExists) throw new Exception("El paciente no existe en el sistema.");

        // 2. Validar que el Médico exista
        var doctorExists = await _context.Doctors.AnyAsync(d => d.Id == request.DoctorId, cancellationToken);
        if (!doctorExists) throw new Exception("El médico no existe en el sistema.");

        // Extraer el día y la hora de la fecha solicitada
        var requestDay = request.ScheduledDate.DayOfWeek;
        var requestTime = TimeOnly.FromDateTime(request.ScheduledDate);

        // 3. Validar Disponibilidad (¿El médico atiende ese día y a esa hora?)
        var isWithinSchedule = await _context.DoctorSchedules.AnyAsync(s => 
            s.DoctorId == request.DoctorId &&
            s.DayOfWeek == requestDay &&
            requestTime >= s.StartTime && requestTime < s.EndTime, 
            cancellationToken);

        if (!isWithinSchedule) 
            throw new Exception("El médico no tiene configurado un horario de atención para el día y hora solicitados.");

        // 4. Validar Doble Reserva (¿Ya tiene una cita a esa hora exacta que no esté cancelada?)
        var isDoubleBooked = await _context.Appointments.AnyAsync(a => 
            a.DoctorId == request.DoctorId && 
            a.ScheduledDate == request.ScheduledDate &&
            a.Status != AppointmentStatus.Cancelled, 
            cancellationToken);

        if (isDoubleBooked) 
            throw new Exception("El médico ya tiene una cita reservada en ese horario exacto.");

        // 5. Mapear y Guardar
        var appointment = new Appointment
        {
            PatientId = request.PatientId,
            DoctorId = request.DoctorId,
            ScheduledDate = request.ScheduledDate,
            Status = AppointmentStatus.Scheduled, // Usamos el Enum nativo de PostgreSQL
            Notes = request.Notes
        };

        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync(cancellationToken);

        // (Opcional pero recomendado: Aquí podríamos disparar un AppointmentBookedEvent a RabbitMQ)

        return appointment.Id;
    }
}