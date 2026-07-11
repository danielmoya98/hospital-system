namespace HospitalSystem.Domain.Entities;

public class TrackingLog
{
    public Guid Id { get; set; }
    
    public Guid ShipmentId { get; set; }
    public Shipment Shipment { get; set; } = null!;

    public DateTime Timestamp { get; set; }
    
    // Coordenadas para telemetría en tiempo real
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double Altitude { get; set; }
    
    // Telemetría del vehículo/drone
    public double BatteryLevel { get; set; }
    public double CurrentSpeed { get; set; }
    public string StatusMessage { get; set; } = string.Empty;
}