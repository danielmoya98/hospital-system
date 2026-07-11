using HospitalSystem.Application.MedicalRecords.Commands.CreateMedicalRecord;
using HospitalSystem.Application.MedicalRecords.Commands.UploadAttachment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalSystem.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MedicalRecordsController : ControllerBase
{
    private readonly IMediator _mediator;

    public MedicalRecordsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRecord([FromBody] CreateMedicalRecordCommand command)
    {
        try
        {
            var recordId = await _mediator.Send(command);
            return Ok(new { Message = "Historial guardado. Cita marcada como completada.", RecordId = recordId });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpPost("{id:guid}/upload")]
    public async Task<IActionResult> UploadAttachment(Guid id, IFormFile file)
    {
        if (file == null || file.Length == 0) return BadRequest("Debe enviar un archivo válido.");

        try
        {
            // Extraemos el stream físico en la capa API para mandarlo puro a la capa Application
            await using var stream = file.OpenReadStream();
            var command = new UploadAttachmentCommand(id, file.FileName, stream, file.ContentType);
            
            var fileUrl = await _mediator.Send(command);
            
            return Ok(new { Message = "Archivo subido con éxito", Url = fileUrl });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
}