using HospitalSystem.Application.Common.Interfaces;
using HospitalSystem.Domain.Entities;
using MediatR;

namespace HospitalSystem.Application.ImagingStudies.Commands.CreateImagingStudy;

public class CreateImagingStudyCommandHandler : IRequestHandler<CreateImagingStudyCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateImagingStudyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateImagingStudyCommand request, CancellationToken cancellationToken)
    {
        var study = new ImagingStudy
        {
            PatientId = request.PatientId,
            DoctorId = request.DoctorId,
            Modality = request.Modality,
            BodyPart = request.BodyPart,
            DiagnosticReport = request.DiagnosticReport,
            StudyDate = DateTime.UtcNow
        };

        _context.ImagingStudies.Add(study);
        await _context.SaveChangesAsync(cancellationToken);

        return study.Id;
    }
}