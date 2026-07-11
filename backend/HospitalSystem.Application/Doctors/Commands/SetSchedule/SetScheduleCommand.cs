using MediatR;

namespace HospitalSystem.Application.Doctors.Commands.SetSchedule;

public record SetScheduleCommand(
    Guid DoctorId,
    DayOfWeek DayOfWeek,
    TimeOnly StartTime,
    TimeOnly EndTime,
    int SlotDurationMinutes
) : IRequest<Guid>;