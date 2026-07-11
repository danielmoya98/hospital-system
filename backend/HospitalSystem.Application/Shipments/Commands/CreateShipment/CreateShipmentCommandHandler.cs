using HospitalSystem.Application.Common.Interfaces;
using HospitalSystem.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.Application.Shipments.Commands.CreateShipment;

public class CreateShipmentCommandHandler : IRequestHandler<CreateShipmentCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateShipmentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateShipmentCommand request, CancellationToken cancellationToken)
    {
        // Validar que la muestra biológica exista
        var sampleExists = await _context.BiologicalSamples.AnyAsync(s => s.Id == request.BiologicalSampleId, cancellationToken);
        if (!sampleExists) throw new Exception("La muestra biológica especificada no existe.");

        var shipment = new Shipment
        {
            BiologicalSampleId = request.BiologicalSampleId,
            TrackingCode = $"SR-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 5).ToUpper()}",
            OriginHospital = request.OriginHospital,
            DestinationHospital = request.DestinationHospital,
            Status = "Dispatched",
            DispatchTime = DateTime.UtcNow
        };

        _context.Shipments.Add(shipment);
        await _context.SaveChangesAsync(cancellationToken);

        return shipment.Id;
    }
}