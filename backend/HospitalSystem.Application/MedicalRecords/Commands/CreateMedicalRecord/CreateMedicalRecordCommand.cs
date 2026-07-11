using MediatR;

namespace HospitalSystem.Application.MedicalRecords.Commands.CreateMedicalRecord;

public record CreateMedicalRecordCommand(
    Guid AppointmentId,
    string Diagnosis,
    string Treatment,
    string? Notes
) : IRequest<Guid>;