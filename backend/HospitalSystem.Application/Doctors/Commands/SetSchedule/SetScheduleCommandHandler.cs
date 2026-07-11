using HospitalSystem.Application.Common.Interfaces;
using HospitalSystem.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.Application.Doctors.Commands.SetSchedule;

public class SetScheduleCommandHandler : IRequestHandler<SetScheduleCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public SetScheduleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(SetScheduleCommand request, CancellationToken cancellationToken)
    {
        // 1. Validar que las horas tengan sentido lógico
        if (request.StartTime >= request.EndTime)
        {
            throw new Exception("La hora de inicio debe ser menor a la hora de finalización.");
        }

        // 2. Validar que el médico exista
        var doctorExists = await _context.Doctors.AnyAsync(d => d.Id == request.DoctorId, cancellationToken);
        if (!doctorExists)
        {
            throw new Exception("El médico especificado no existe.");
        }

        // 3. Validar Solapamiento de Horarios (Overlap)
        // Buscamos si hay algún horario existente que choque con el rango solicitado
        var hasOverlap = await _context.DoctorSchedules
            .AnyAsync(s => s.DoctorId == request.DoctorId &&
                           s.DayOfWeek == request.DayOfWeek &&
                           ((request.StartTime >= s.StartTime && request.StartTime < s.EndTime) ||
                            (request.EndTime > s.StartTime && request.EndTime <= s.EndTime) ||
                            (request.StartTime <= s.StartTime && request.EndTime >= s.EndTime)),
                     cancellationToken);

        if (hasOverlap)
        {
            throw new Exception("El horario solicitado se solapa con una agenda existente para este médico.");
        }

        // 4. Mapear y guardar en la base de datos
        var schedule = new DoctorSchedule
        {
            DoctorId = request.DoctorId,
            DayOfWeek = request.DayOfWeek,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            SlotDurationMinutes = request.SlotDurationMinutes
        };

        _context.DoctorSchedules.Add(schedule);
        await _context.SaveChangesAsync(cancellationToken);

        return schedule.Id;
    }
}