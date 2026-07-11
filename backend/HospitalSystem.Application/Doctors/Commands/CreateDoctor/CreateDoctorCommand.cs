using MediatR;

namespace HospitalSystem.Application.Doctors.Commands.CreateDoctor;

public record CreateDoctorCommand(
    string Username,
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string LicenseNumber,
    string? PhoneNumber,
    Guid SpecialtyId
) : IRequest<Guid>;