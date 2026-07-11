using HospitalSystem.Domain.Common;

namespace HospitalSystem.Domain.Entities;

public class Doctor : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    public Guid SpecialtyId { get; set; }
    public Specialty Specialty { get; set; } = null!;

    public string LicenseNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }

    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
}