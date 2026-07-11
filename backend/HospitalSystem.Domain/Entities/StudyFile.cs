namespace HospitalSystem.Domain.Entities;

public class StudyFile
{
    public Guid Id { get; set; }
    
    public Guid ImagingStudyId { get; set; }
    public ImagingStudy ImagingStudy { get; set; } = null!;

    // Punteros directos a MinIO
    public string BucketName { get; set; } = "pacs-studies";
    public string ObjectKey { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty; // ej: application/dicom, image/jpeg
    public long FileSizeBytes { get; set; }
}