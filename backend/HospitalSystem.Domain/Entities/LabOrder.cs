namespace HospitalSystem.Domain.Entities;

public class LabOrder
{
    public Guid Id { get; set; }
    
    public Guid PatientId { get; set; }
    public Patient Patient { get; set; } = null!;
    
    public Guid DoctorId { get; set; }
    public Doctor Doctor { get; set; } = null!;

    public DateTime OrderDate { get; set; }
    public string TestType { get; set; } = string.Empty; // Ej: Hemograma, Perfil Lipídico
    public string Status { get; set; } = "Pending"; // Pending, InProgress, Completed
    public string? ClinicalNotes { get; set; }
    
    public ICollection<LabResult> Results { get; set; } = new List<LabResult>();
}