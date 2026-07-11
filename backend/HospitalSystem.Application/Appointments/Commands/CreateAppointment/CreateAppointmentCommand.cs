using MediatR;

namespace HospitalSystem.Application.Appointments.Commands.CreateAppointment;

public record CreateAppointmentCommand(
    Guid PatientId,
    Guid DoctorId,
    DateTime ScheduledDate,
    string? Notes
) : IRequest<Guid>;