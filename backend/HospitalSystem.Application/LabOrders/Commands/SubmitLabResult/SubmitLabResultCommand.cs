using MediatR;

namespace HospitalSystem.Application.LabOrders.Commands.SubmitLabResult;

public record SubmitLabResultCommand(
    Guid LabOrderId,
    string ResultData,
    string? ReferenceValues,
    bool IsAbnormal
) : IRequest<Guid>;