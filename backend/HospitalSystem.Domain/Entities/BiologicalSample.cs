namespace HospitalSystem.Domain.Entities;

public class BiologicalSample
{
    public Guid Id { get; set; }
    
    public Guid PatientId { get; set; }
    public Patient Patient { get; set; } = null!;

    public string SampleType { get; set; } = string.Empty; // Sangre, Tejido, etc.
    public DateTime ExtractionDate { get; set; }
    public string StorageConditions { get; set; } = string.Empty; // Ej: Refrigerado 2-8°C
}