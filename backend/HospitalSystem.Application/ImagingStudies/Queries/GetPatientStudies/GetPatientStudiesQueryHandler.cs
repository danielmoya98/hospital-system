using HospitalSystem.Application.Common.Interfaces;
using HospitalSystem.Application.ImagingStudies.Queries.GetPatientStudies;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.Application.ImagingStudies.Queries.GetPatientStudies;

public class GetPatientStudiesQueryHandler : IRequestHandler<GetPatientStudiesQuery, List<PatientStudyDto>>
{
    private readonly IApplicationDbContext _context;

    public GetPatientStudiesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<PatientStudyDto>> Handle(GetPatientStudiesQuery request, CancellationToken cancellationToken)
    {
        return await _context.ImagingStudies
            .AsNoTracking()
            .Where(s => s.PatientId == request.PatientId)
            .Include(s => s.Files)
            .Select(s => new PatientStudyDto(
                s.Id,
                s.Modality,
                s.BodyPart,
                s.StudyDate,
                s.DiagnosticReport,
                s.Files.Select(f => new StudyFileLookupDto(f.Id, f.FileUrl, f.ContentType, f.FileSizeBytes)).ToList()
            ))
            .ToListAsync(cancellationToken);
    }
}