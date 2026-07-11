using HospitalSystem.Application.Shipments.Commands.CreateShipment;
using HospitalSystem.Application.Shipments.Commands.IngestTelemetry;
using HospitalSystem.Application.Shipments.Queries.GetShipmentRoute;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalSystem.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ShipmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ShipmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateShipment([FromBody] CreateShipmentCommand command)
    {
        try
        {
            var shipmentId = await _mediator.Send(command);
            return Ok(new { Message = "Despacho logístico iniciado.", ShipmentId = shipmentId });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpPost("{id:guid}/telemetry")]
    public async Task<IActionResult> IngestTelemetry(Guid id, [FromBody] IngestTelemetryCommand command)
    {
        if (id != command.ShipmentId) return BadRequest("El ID del envío no coincide.");

        try
        {
            var logId = await _mediator.Send(command);
            return Ok(new { Message = "Telemetría recibida.", LogId = logId });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpGet("{id:guid}/route")]
    public async Task<IActionResult> GetRoute(Guid id)
    {
        var route = await _mediator.Send(new GetShipmentRouteQuery(id));
        return Ok(route);
    }
}