using HospitalSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.Application.MedicalRecords.Commands.UploadAttachment;

public class UploadAttachmentCommandHandler : IRequestHandler<UploadAttachmentCommand, string>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStorageService _fileStorageService;

    public UploadAttachmentCommandHandler(IApplicationDbContext context, IFileStorageService fileStorageService)
    {
        _context = context;
        _fileStorageService = fileStorageService;
    }

    public async Task<string> Handle(UploadAttachmentCommand request, CancellationToken cancellationToken)
    {
        var record = await _context.MedicalRecords.FirstOrDefaultAsync(r => r.Id == request.MedicalRecordId, cancellationToken);
        if (record == null) throw new Exception("Historial clínico no encontrado.");

        // Subir el archivo a MinIO (al bucket "medical-records")
        var fileUrl = await _fileStorageService.UploadFileAsync(
            "medical-records", 
            $"{record.Id}/{request.FileName}", 
            request.FileStream, 
            request.ContentType, 
            cancellationToken);

        // Guardar la URL en la base de datos
        record.AttachmentUrl = fileUrl;
        await _context.SaveChangesAsync(cancellationToken);

        return fileUrl;
    }
}