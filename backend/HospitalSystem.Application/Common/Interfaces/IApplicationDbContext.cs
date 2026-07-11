using HospitalSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    // Tablas de Seguridad / Administrativas
    DbSet<Role> Roles { get; set; }
    DbSet<User> Users { get; set; }
    DbSet<Specialty> Specialties { get; set; }
    
    // Tablas Clínicas
    DbSet<Patient> Patients { get; set; }
    DbSet<Doctor> Doctors { get; set; }
    DbSet<Appointment> Appointments { get; set; }
    DbSet<MedicalRecord> MedicalRecords { get; set; }
    DbSet<DoctorSchedule> DoctorSchedules { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    
    // Módulo de Laboratorio
    DbSet<LabOrder> LabOrders { get; set; }
    DbSet<LabResult> LabResults { get; set; }

// Módulo de Radiología / MinIO
    DbSet<ImagingStudy> ImagingStudies { get; set; }
    DbSet<StudyFile> StudyFiles { get; set; }

// Módulo de Logística y Telemetría
    DbSet<BiologicalSample> BiologicalSamples { get; set; }
    DbSet<Shipment> Shipments { get; set; }
    DbSet<TrackingLog> TrackingLogs { get; set; }
}