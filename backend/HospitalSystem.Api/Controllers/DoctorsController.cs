using HospitalSystem.Application.Doctors.Commands.CreateDoctor;
using HospitalSystem.Application.Doctors.Commands.SetSchedule;
using HospitalSystem.Application.Doctors.Queries.GetDoctorsBySpecialty;
using HospitalSystem.Application.Doctors.Queries.GetDoctorSchedule;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalSystem.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class DoctorsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DoctorsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // --- COMMANDS (Escrituras) ---

    [HttpPost]
    public async Task<IActionResult> CreateDoctor([FromBody] CreateDoctorCommand command)
    {
        try
        {
            var doctorId = await _mediator.Send(command);
            return Ok(new { Message = "Médico registrado con éxito", DoctorId = doctorId });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpPost("schedule")]
    public async Task<IActionResult> SetSchedule([FromBody] SetScheduleCommand command)
    {
        try
        {
            var scheduleId = await _mediator.Send(command);
            return Ok(new { Message = "Horario configurado con éxito", ScheduleId = scheduleId });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    // --- QUERIES (Lecturas) ---

    [HttpGet("by-specialty/{specialtyId:guid}")]
    public async Task<IActionResult> GetDoctorsBySpecialty(Guid specialtyId)
    {
        var query = new GetDoctorsBySpecialtyQuery(specialtyId);
        var doctors = await _mediator.Send(query);
        return Ok(doctors);
    }

    [HttpGet("{doctorId:guid}/schedule")]
    public async Task<IActionResult> GetDoctorSchedule(Guid doctorId)
    {
        var query = new GetDoctorScheduleQuery(doctorId);
        var schedule = await _mediator.Send(query);
        return Ok(schedule);
    }
}