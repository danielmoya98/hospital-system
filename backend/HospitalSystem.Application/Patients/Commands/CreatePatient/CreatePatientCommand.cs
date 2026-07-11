using MediatR;
using HospitalSystem.Domain.Enums;

namespace HospitalSystem.Application.Patients.Commands.CreatePatient;

// Usamos un 'record' porque los comandos deben ser inmutables.
// Devuelve un Guid (el ID del paciente creado).
public record CreatePatientCommand(
    string NationalId,
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    GenderType Gender,
    string? PhoneNumber,
    string? Email
) : IRequest<Guid>;