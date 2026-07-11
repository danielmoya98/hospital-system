using HospitalSystem.Domain.Common;

namespace HospitalSystem.Domain.Entities;

public class Specialty : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    
    public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
}