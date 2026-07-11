using MediatR;

namespace HospitalSystem.Application.Specialties.Commands.CreateSpecialty;

public record CreateSpecialtyCommand(string Name, string? Description) : IRequest<Guid>;