using HospitalSystem.Domain.Common;

namespace HospitalSystem.Domain.Entities;

public class User : BaseEntity
{
    public Guid RoleId { get; set; }
    public Role Role { get; set; } = null!; // Propiedad de navegación

    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    
    // Si el usuario es un Doctor, esta es su relación 1 a 1 (opcional para el admin/staff)
    public Doctor? DoctorProfile { get; set; }
}