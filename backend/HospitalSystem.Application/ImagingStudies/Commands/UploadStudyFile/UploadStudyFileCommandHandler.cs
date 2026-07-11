using HospitalSystem.Application.Common.Interfaces;
using HospitalSystem.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.Application.ImagingStudies.Commands.UploadStudyFile;

public class UploadStudyFileCommandHandler : IRequestHandler<UploadStudyFileCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStorageService _fileStorageService;

    public UploadStudyFileCommandHandler(IApplicationDbContext context, IFileStorageService fileStorageService)
    {
        _context = context;
        _fileStorageService = fileStorageService;
    }

    public async Task<Guid> Handle(UploadStudyFileCommand request, CancellationToken cancellationToken)
    {
        // Buscamos el estudio en PostgreSQL
        var study = await _context.ImagingStudies
            .FirstOrDefaultAsync(s => s.Id == request.ImagingStudyId, cancellationToken);
            
        if (study == null) throw new Exception("Estudio radiológico no encontrado.");

        const string bucket = "pacs-studies";
        var uniqueFileName = $"{study.Id}/{Guid.NewGuid()}_{request.FileName}";

        // 1. Guardar archivo físico en MinIO usando el servicio de la Fase 2
        var fileUrl = await _fileStorageService.UploadFileAsync(
            bucket,
            uniqueFileName,
            request.FileStream,
            request.ContentType,
            cancellationToken);

        // 2. Registrar metadatos en la tabla StudyFiles en PostgreSQL
        var studyFile = new StudyFile
        {
            ImagingStudyId = request.ImagingStudyId,
            BucketName = bucket,
            ObjectKey = uniqueFileName,
            FileUrl = fileUrl,
            ContentType = request.ContentType,
            FileSizeBytes = request.FileSizeBytes
        };

        _context.StudyFiles.Add(studyFile);
        await _context.SaveChangesAsync(cancellationToken);

        return studyFile.Id;
    }
}