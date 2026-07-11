using HospitalSystem.Domain.Entities;

namespace HospitalSystem.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user, Role role);
}