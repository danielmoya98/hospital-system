using MediatR;

namespace HospitalSystem.Application.MedicalRecords.Commands.UploadAttachment;

// Nota: No pasamos IFormFile aquí para no acoplar Application a la web. Pasamos un Stream.
public record UploadAttachmentCommand(
    Guid MedicalRecordId,
    string FileName,
    Stream FileStream,
    string ContentType
) : IRequest<string>;