namespace HospitalSystem.Application.Doctors.Queries.DTOs;

public record DoctorScheduleDto(
    Guid Id,
    DayOfWeek DayOfWeek,
    TimeOnly StartTime,
    TimeOnly EndTime,
    int SlotDurationMinutes
);