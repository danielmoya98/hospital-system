using HospitalSystem.Domain.Common;
using HospitalSystem.Domain.Enums;

namespace HospitalSystem.Domain.Entities;

public class Patient : BaseEntity
{
    public string NationalId { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    
    // Usamos DateOnly para que coincida perfectamente con el DATE de PostgreSQL
    public DateOnly DateOfBirth { get; set; } 
    
    public GenderType Gender { get; set; }
    public BloodType? BloodGroup { get; set; }
    
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }

    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
}