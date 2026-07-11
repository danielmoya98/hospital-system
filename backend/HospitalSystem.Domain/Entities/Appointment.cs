using HospitalSystem.Domain.Common;
using HospitalSystem.Domain.Enums;

namespace HospitalSystem.Domain.Entities;

public class Appointment : BaseEntity
{
    public Guid PatientId { get; set; }
    public Patient Patient { get; set; } = null!;

    public Guid DoctorId { get; set; }
    public Doctor Doctor { get; set; } = null!;

    public DateTimeOffset ScheduledDate { get; set; }
    public string ReasonForVisit { get; set; } = string.Empty;
    
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
    public string? Notes { get; set; }
    
    public MedicalRecord? MedicalRecord { get; set; } // 1 a 1 o 1 a 0
}