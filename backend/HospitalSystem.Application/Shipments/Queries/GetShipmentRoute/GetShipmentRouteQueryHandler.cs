using HospitalSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.Application.Shipments.Queries.GetShipmentRoute;

public class GetShipmentRouteQueryHandler : IRequestHandler<GetShipmentRouteQuery, List<RoutePointDto>>
{
    private readonly IApplicationDbContext _context;

    public GetShipmentRouteQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<RoutePointDto>> Handle(GetShipmentRouteQuery request, CancellationToken cancellationToken)
    {
        return await _context.TrackingLogs
            .AsNoTracking()
            .Where(t => t.ShipmentId == request.ShipmentId)
            .OrderBy(t => t.Timestamp)
            .Select(t => new RoutePointDto(
                t.Latitude,
                t.Longitude,
                t.Altitude,
                t.BatteryLevel,
                t.CurrentSpeed,
                t.Timestamp,
                t.StatusMessage))
            .ToListAsync(cancellationToken);
    }
}