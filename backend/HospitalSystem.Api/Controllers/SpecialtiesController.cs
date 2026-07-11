using HospitalSystem.Application.Specialties.Commands.CreateSpecialty;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalSystem.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SpecialtiesController : ControllerBase
{
    private readonly IMediator _mediator;

    public SpecialtiesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateSpecialty([FromBody] CreateSpecialtyCommand command)
    {
        var specialtyId = await _mediator.Send(command);
        
        return Ok(new { Message = "Especialidad registrada con éxito", SpecialtyId = specialtyId });
    }
}