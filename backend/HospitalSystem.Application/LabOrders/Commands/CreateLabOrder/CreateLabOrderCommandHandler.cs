using HospitalSystem.Application.Common.Interfaces;
using HospitalSystem.Domain.Entities;
using MediatR;

namespace HospitalSystem.Application.LabOrders.Commands.CreateLabOrder;

public class CreateLabOrderCommandHandler : IRequestHandler<CreateLabOrderCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateLabOrderCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateLabOrderCommand request, CancellationToken cancellationToken)
    {
        var labOrder = new LabOrder
        {
            PatientId = request.PatientId,
            DoctorId = request.DoctorId,
            TestType = request.TestType,
            ClinicalNotes = request.ClinicalNotes,
            OrderDate = DateTime.UtcNow,
            Status = "Pending"
        };

        _context.LabOrders.Add(labOrder);
        await _context.SaveChangesAsync(cancellationToken);

        return labOrder.Id;
    }
}