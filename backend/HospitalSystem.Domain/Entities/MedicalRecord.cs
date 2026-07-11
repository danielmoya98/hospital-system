namespace HospitalSystem.Domain.Entities;

public class MedicalRecord
{
    public Guid Id { get; set; }
    
    // Relaciones
    public Guid PatientId { get; set; }
    public Patient Patient { get; set; } = null!;
    
    public Guid DoctorId { get; set; }
    public Doctor Doctor { get; set; } = null!;
    
    public Guid AppointmentId { get; set; }
    public Appointment Appointment { get; set; } = null!;

    // Datos Clínicos
    public string Diagnosis { get; set; } = string.Empty;
    public string Treatment { get; set; } = string.Empty;
    public string? Notes { get; set; }
    
    // Almacenamiento de Archivos (MinIO)
    public string? AttachmentUrl { get; set; }
}