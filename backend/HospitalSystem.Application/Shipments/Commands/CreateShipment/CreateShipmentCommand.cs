using MediatR;

namespace HospitalSystem.Application.Shipments.Commands.CreateShipment;

public record CreateShipmentCommand(
    Guid BiologicalSampleId,
    string OriginHospital,
    string DestinationHospital
) : IRequest<Guid>;