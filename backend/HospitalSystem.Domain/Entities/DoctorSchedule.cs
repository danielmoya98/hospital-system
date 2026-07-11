namespace HospitalSystem.Domain.Entities;

public class DoctorSchedule
{
    public Guid Id { get; set; }
    
    // Relación con el Médico
    public Guid DoctorId { get; set; }
    public Doctor Doctor { get; set; } = null!;

    // Datos del Horario
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int SlotDurationMinutes { get; set; }
}