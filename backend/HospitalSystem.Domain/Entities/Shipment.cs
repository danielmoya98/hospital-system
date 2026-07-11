namespace HospitalSystem.Domain.Entities;

public class Shipment
{
    public Guid Id { get; set; }
    
    public Guid BiologicalSampleId { get; set; }
    public BiologicalSample BiologicalSample { get; set; } = null!;

    public string TrackingCode { get; set; } = string.Empty;
    public string OriginHospital { get; set; } = string.Empty;
    public string DestinationHospital { get; set; } = string.Empty;
    public string Status { get; set; } = "Dispatched"; // Dispatched, InTransit, Delivered, Failed
    public DateTime DispatchTime { get; set; }
    public DateTime? EstimatedArrivalTime { get; set; }
    
    public ICollection<TrackingLog> TrackingLogs { get; set; } = new List<TrackingLog>();
}