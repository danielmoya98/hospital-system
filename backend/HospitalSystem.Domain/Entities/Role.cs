using HospitalSystem.Domain.Common;

namespace HospitalSystem.Domain.Entities;

public class Role : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    
    // Propiedad de navegación (Un rol tiene muchos usuarios)
    public ICollection<User> Users { get; set; } = new List<User>();
}