namespace HospitalSystem.Application.Doctors.Queries.DTOs;

public record DoctorDto(
    Guid Id,
    string FirstName,
    string LastName,
    string LicenseNumber,
    Guid SpecialtyId
);