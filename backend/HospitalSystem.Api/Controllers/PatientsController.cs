using HospitalSystem.Application.Patients.Commands.CreatePatient;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalSystem.Api.Controllers;

[Authorize] // <-- Candado activado para todo el controlador
[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PatientsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePatient([FromBody] CreatePatientCommand command)
    {
        // Enviamos el comando a MediatR. Él buscará automáticamente el Handler.
        var patientId = await _mediator.Send(command);
        
        return Ok(new { Message = "Paciente registrado con éxito", PatientId = patientId });
    }
}