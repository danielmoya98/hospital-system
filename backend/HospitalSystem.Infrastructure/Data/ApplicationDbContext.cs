using HospitalSystem.Domain.Entities;
using HospitalSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using HospitalSystem.Application.Common.Interfaces;

namespace HospitalSystem.Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // Declaración de las tablas (DbSets)
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Specialty> Specialties { get; set; } = null!;
    public DbSet<Doctor> Doctors { get; set; } = null!;
    public DbSet<Patient> Patients { get; set; } = null!;
    public DbSet<Appointment> Appointments { get; set; } = null!;
    public DbSet<MedicalRecord> MedicalRecords { get; set; } = null!;
    public DbSet<DoctorSchedule> DoctorSchedules { get; set; } = null!;
    
    public DbSet<LabOrder> LabOrders { get; set; } = null!;
    public DbSet<LabResult> LabResults { get; set; } = null!;
    public DbSet<ImagingStudy> ImagingStudies { get; set; } = null!;
    public DbSet<StudyFile> StudyFiles { get; set; } = null!;
    public DbSet<BiologicalSample> BiologicalSamples { get; set; } = null!;
    public DbSet<Shipment> Shipments { get; set; } = null!;
    public DbSet<TrackingLog> TrackingLogs { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 1. Mapeo nativo de los ENUMs de PostgreSQL
        modelBuilder.HasPostgresEnum<AppointmentStatus>();
        modelBuilder.HasPostgresEnum<GenderType>();
        modelBuilder.HasPostgresEnum<BloodType>();

        // 2. Configuraciones globales (Convenciones)
        // PostgreSQL suele usar snake_case para las tablas, pero mantendremos 
        // los nombres exactos de tu script SQL.

        // 3. Configuraciones específicas (Fluent API) para coincidir con tu SQL
        
        // Roles
        modelBuilder.Entity<Role>()
            .HasIndex(r => r.Name)
            .IsUnique();

        // Users
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Doctors
        modelBuilder.Entity<Doctor>()
            .HasIndex(d => d.LicenseNumber)
            .IsUnique();
        modelBuilder.Entity<Doctor>()
            .HasOne(d => d.User)
            .WithOne(u => u.DoctorProfile)
            .HasForeignKey<Doctor>(d => d.UserId)
            .OnDelete(DeleteBehavior.Cascade); // Mismo comportamiento que tu SQL

        // Patients
        modelBuilder.Entity<Patient>()
            .HasIndex(p => p.NationalId)
            .IsUnique();

        // Appointments (Índices para búsquedas de horarios)
        modelBuilder.Entity<Appointment>()
            .HasIndex(a => a.ScheduledDate);
        modelBuilder.Entity<Appointment>()
            .HasIndex(a => new { a.DoctorId, a.ScheduledDate });

        // MedicalRecords
        modelBuilder.Entity<MedicalRecord>()
            .HasIndex(m => m.PatientId);
    }
}