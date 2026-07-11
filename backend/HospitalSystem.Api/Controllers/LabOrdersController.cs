using HospitalSystem.Application.LabOrders.Commands.CreateLabOrder;
using HospitalSystem.Application.LabOrders.Commands.SubmitLabResult;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalSystem.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class LabOrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public LabOrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateLabOrderCommand command)
    {
        var orderId = await _mediator.Send(command);
        return Ok(new { Message = "Orden de laboratorio creada", OrderId = orderId });
    }

    [HttpPost("{id:guid}/results")]
    public async Task<IActionResult> SubmitResult(Guid id, [FromBody] SubmitLabResultCommand command)
    {
        if (id != command.LabOrderId) return BadRequest("El ID de la ruta no coincide con el cuerpo.");
        
        var resultId = await _mediator.Send(command);
        return Ok(new { Message = "Resultado guardado y notificado", ResultId = resultId });
    }
}