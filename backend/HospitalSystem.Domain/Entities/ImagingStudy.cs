namespace HospitalSystem.Domain.Entities;

public class ImagingStudy
{
    public Guid Id { get; set; }
    
    public Guid PatientId { get; set; }
    public Patient Patient { get; set; } = null!;
    
    public Guid DoctorId { get; set; }
    public Doctor Doctor { get; set; } = null!;

    public string Modality { get; set; } = string.Empty; // X-RAY, MRI, CT-SCAN
    public string BodyPart { get; set; } = string.Empty;
    public DateTime StudyDate { get; set; }
    public string? DiagnosticReport { get; set; }
    
    public ICollection<StudyFile> Files { get; set; } = new List<StudyFile>();
}