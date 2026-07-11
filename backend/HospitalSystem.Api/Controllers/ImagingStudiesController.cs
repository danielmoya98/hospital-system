using HospitalSystem.Application.ImagingStudies.Commands.CreateImagingStudy;
using HospitalSystem.Application.ImagingStudies.Commands.UploadStudyFile;
using HospitalSystem.Application.ImagingStudies.Queries.GetPatientStudies;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalSystem.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ImagingStudiesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ImagingStudiesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateStudy([FromBody] CreateImagingStudyCommand command)
    {
        var studyId = await _mediator.Send(command);
        return Ok(new { Message = "Estudio radiológico registrado.", ImagingStudyId = studyId });
    }

    [HttpPost("{id:guid}/upload")]
    public async Task<IActionResult> UploadFile(Guid id, IFormFile file)
    {
        if (file == null || file.Length == 0) return BadRequest("Archivo de imagen no válido.");

        try
        {
            await using var stream = file.OpenReadStream();
            var command = new UploadStudyFileCommand(id, file.FileName, stream, file.ContentType, file.Length);
            
            var fileId = await _mediator.Send(command);
            return Ok(new { Message = "Placa médica guardada en PACS (MinIO)", FileId = fileId });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpGet("patient/{patientId:guid}")]
    public async Task<IActionResult> GetPatientStudies(Guid patientId)
    {
        var studies = await _mediator.Send(new GetPatientStudiesQuery(patientId));
        return Ok(studies);
    }
}