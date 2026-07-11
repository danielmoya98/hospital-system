using HospitalSystem.Application.Common.Events;
using MassTransit;

namespace HospitalSystem.Api.Consumers;

public class PatientRegisteredEventConsumer : IConsumer<PatientRegisteredEvent>
{
    private readonly ILogger<PatientRegisteredEventConsumer> _logger;

    public PatientRegisteredEventConsumer(ILogger<PatientRegisteredEventConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<PatientRegisteredEvent> context)
    {
        var evento = context.Message;

        // Aquí iría tu lógica real de envío de emails (ej. con SendGrid o SMTP)
        _logger.LogInformation("==================================================");
        _logger.LogInformation("🐰 [RABBITMQ] Evento Consumido Exitosamente");
        _logger.LogInformation($"📧 Simulando envío de correo de bienvenida...");
        _logger.LogInformation($"👤 Paciente: {evento.FullName} (CI: {evento.NationalId})");
        _logger.LogInformation($"✉️ Destino: {evento.Email ?? "Sin correo registrado"}");
        _logger.LogInformation("==================================================");

        return Task.CompletedTask;
    }
}