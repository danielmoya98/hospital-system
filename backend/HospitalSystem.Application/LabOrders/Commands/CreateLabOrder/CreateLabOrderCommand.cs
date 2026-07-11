using MediatR;

namespace HospitalSystem.Application.LabOrders.Commands.CreateLabOrder;

public record CreateLabOrderCommand(
    Guid PatientId,
    Guid DoctorId,
    string TestType,
    string? ClinicalNotes
) : IRequest<Guid>;