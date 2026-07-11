using MediatR;

namespace HospitalSystem.Application.Shipments.Queries.GetShipmentRoute;

public record RoutePointDto(
    double Latitude, 
    double Longitude, 
    double Altitude, 
    double BatteryLevel, 
    double CurrentSpeed, 
    DateTime Timestamp, 
    string StatusMessage
);

public record GetShipmentRouteQuery(Guid ShipmentId) : IRequest<List<RoutePointDto>>;