using MediatR;

namespace HospitalSystem.Application.Shipments.Commands.IngestTelemetry;

public record IngestTelemetryCommand(
    Guid ShipmentId,
    double Latitude,
    double Longitude,
    double Altitude,
    double BatteryLevel,
    double CurrentSpeed,
    string StatusMessage
) : IRequest<Guid>;