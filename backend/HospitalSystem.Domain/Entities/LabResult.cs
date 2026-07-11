namespace HospitalSystem.Domain.Entities;

public class LabResult
{
    public Guid Id { get; set; }
    
    public Guid LabOrderId { get; set; }
    public LabOrder LabOrder { get; set; } = null!;

    public string ResultData { get; set; } = string.Empty; // Puede ser un JSON con los biomarcadores
    public string? ReferenceValues { get; set; }
    public bool IsAbnormal { get; set; }
    public DateTime ProcessedAt { get; set; }
}