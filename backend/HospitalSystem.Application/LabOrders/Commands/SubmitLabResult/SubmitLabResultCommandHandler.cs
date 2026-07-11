using HospitalSystem.Application.Common.Interfaces;
using HospitalSystem.Application.Common.Events;
using HospitalSystem.Domain.Entities;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.Application.LabOrders.Commands.SubmitLabResult;

public class SubmitLabResultCommandHandler : IRequestHandler<SubmitLabResultCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IPublishEndpoint _publishEndpoint;

    public SubmitLabResultCommandHandler(IApplicationDbContext context, IPublishEndpoint publishEndpoint)
    {
        _context = context;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<Guid> Handle(SubmitLabResultCommand request, CancellationToken cancellationToken)
    {
        var order = await _context.LabOrders.FirstOrDefaultAsync(o => o.Id == request.LabOrderId, cancellationToken);
        if (order == null) throw new Exception("Orden de laboratorio no encontrada.");

        var result = new LabResult
        {
            LabOrderId = request.LabOrderId,
            ResultData = request.ResultData,
            ReferenceValues = request.ReferenceValues,
            IsAbnormal = request.IsAbnormal,
            ProcessedAt = DateTime.UtcNow
        };

        order.Status = "Completed";

        _context.LabResults.Add(result);
        await _context.SaveChangesAsync(cancellationToken);

        // Disparar la notificación asíncrona a RabbitMQ
        await _publishEndpoint.Publish(new LabResultProcessedEvent(
            order.Id,
            order.DoctorId,
            order.TestType,
            result.IsAbnormal
        ), cancellationToken);

        return result.Id;
    }
}