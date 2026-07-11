using MediatR;

namespace HospitalSystem.Application.ImagingStudies.Commands.UploadStudyFile;

public record UploadStudyFileCommand(
    Guid ImagingStudyId,
    string FileName,
    Stream FileStream,
    string ContentType,
    long FileSizeBytes
) : IRequest<Guid>;