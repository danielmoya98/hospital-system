namespace HospitalSystem.Application.Common.Events;

public record LabResultProcessedEvent(
    Guid LabOrderId,
    Guid DoctorId,
    string TestType,
    bool IsAbnormal
);