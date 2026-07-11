using MediatR;

namespace HospitalSystem.Application.ImagingStudies.Commands.CreateImagingStudy;

public record CreateImagingStudyCommand(
    Guid PatientId,
    Guid DoctorId,
    string Modality, // X-RAY, MRI, CT-SCAN
    string BodyPart,
    string? DiagnosticReport
) : IRequest<Guid>;