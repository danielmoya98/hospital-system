using HospitalSystem.Application.Common.Interfaces;
using HospitalSystem.Application.Doctors.Queries.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.Application.Doctors.Queries.GetDoctorsBySpecialty;

public class GetDoctorsBySpecialtyQueryHandler : IRequestHandler<GetDoctorsBySpecialtyQuery, List<DoctorDto>>
{
    private readonly IApplicationDbContext _context;

    public GetDoctorsBySpecialtyQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<DoctorDto>> Handle(GetDoctorsBySpecialtyQuery request, CancellationToken cancellationToken)
    {
        // Usamos AsNoTracking() porque es una consulta de solo lectura, lo que la hace ultra rápida
        return await _context.Doctors
            .AsNoTracking()
            .Where(d => d.SpecialtyId == request.SpecialtyId)
            .Select(d => new DoctorDto(
                d.Id,
                d.FirstName,
                d.LastName,
                d.LicenseNumber,
                d.SpecialtyId))
            .ToListAsync(cancellationToken);
    }
}