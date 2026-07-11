using MediatR;

namespace HospitalSystem.Application.ImagingStudies.Queries.GetPatientStudies;

public record StudyFileLookupDto(Guid Id, string FileUrl, string ContentType, long FileSizeBytes);
public record PatientStudyDto(Guid Id, string Modality, string BodyPart, DateTime StudyDate, string? DiagnosticReport, List<StudyFileLookupDto> Files);

public record GetPatientStudiesQuery(Guid PatientId) : IRequest<List<PatientStudyDto>>;