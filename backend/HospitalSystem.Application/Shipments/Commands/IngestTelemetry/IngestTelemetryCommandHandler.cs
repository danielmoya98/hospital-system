using HospitalSystem.Application.Common.Interfaces;
using HospitalSystem.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.Application.Shipments.Commands.IngestTelemetry;

public class IngestTelemetryCommandHandler : IRequestHandler<IngestTelemetryCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public IngestTelemetryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(IngestTelemetryCommand request, CancellationToken cancellationToken)
    {
        // En un entorno de altísima concurrencia, esto podría ir primero a Redis, 
        // pero para nuestra arquitectura actual, PostgreSQL lo soportará perfectamente.
        
        var shipmentExists = await _context.Shipments.AnyAsync(s => s.Id == request.ShipmentId, cancellationToken);
        if (!shipmentExists) throw new Exception("El envío especificado no existe o ha concluido.");

        var log = new TrackingLog
        {
            ShipmentId = request.ShipmentId,
            Timestamp = DateTime.UtcNow,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            Altitude = request.Altitude,
            BatteryLevel = request.BatteryLevel,
            CurrentSpeed = request.CurrentSpeed,
            StatusMessage = request.StatusMessage
        };

        _context.TrackingLogs.Add(log);
        await _context.SaveChangesAsync(cancellationToken);

        return log.Id;
    }
}