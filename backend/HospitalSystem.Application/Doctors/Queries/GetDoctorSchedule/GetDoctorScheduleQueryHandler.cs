using HospitalSystem.Application.Common.Interfaces;
using HospitalSystem.Application.Doctors.Queries.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.Application.Doctors.Queries.GetDoctorSchedule;

public class GetDoctorScheduleQueryHandler : IRequestHandler<GetDoctorScheduleQuery, List<DoctorScheduleDto>>
{
    private readonly IApplicationDbContext _context;

    public GetDoctorScheduleQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<DoctorScheduleDto>> Handle(GetDoctorScheduleQuery request, CancellationToken cancellationToken)
    {
        return await _context.DoctorSchedules
            .AsNoTracking()
            .Where(s => s.DoctorId == request.DoctorId)
            .Select(s => new DoctorScheduleDto(
                s.Id,
                s.DayOfWeek,
                s.StartTime,
                s.EndTime,
                s.SlotDurationMinutes))
            .ToListAsync(cancellationToken);
    }
}