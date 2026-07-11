using HospitalSystem.Application.Common.Events;
using MassTransit;

namespace HospitalSystem.Api.Consumers;

public class LabResultProcessedEventConsumer : IConsumer<LabResultProcessedEvent>
{
    private readonly ILogger<LabResultProcessedEventConsumer> _logger;

    public LabResultProcessedEventConsumer(ILogger<LabResultProcessedEventConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<LabResultProcessedEvent> context)
    {
        var evento = context.Message;

        _logger.LogInformation("==================================================");
        _logger.LogInformation("🧪 [LABORATORIO - RABBITMQ] Nuevo resultado procesado");
        _logger.LogInformation($"👨‍⚕️ Notificando al Médico ID: {evento.DoctorId}");
        _logger.LogInformation($"🔬 Examen: {evento.TestType}");
        
        if (evento.IsAbnormal)
        {
            _logger.LogWarning("⚠️ ALERTA: Los resultados presentan valores anormales. Requiere atención inmediata.");
        }
        
        _logger.LogInformation("==================================================");

        return Task.CompletedTask;
    }
}