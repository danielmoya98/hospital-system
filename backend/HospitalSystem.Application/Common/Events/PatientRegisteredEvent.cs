namespace HospitalSystem.Application.Common.Events;

public record PatientRegisteredEvent(
    Guid PatientId,
    string NationalId,
    string FullName,
    string? Email
);