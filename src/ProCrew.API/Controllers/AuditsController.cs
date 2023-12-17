using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProCrew.Application;
using ProCrew.Application.Features.Audits.Models;
using ProCrew.Application.Features.Audits.Queries;
using ProCrew.Application.Features.Products.Models;
using ProCrew.Application.Features.Products.Queries;

namespace ProCrew.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuditsController(ISender sender) : ControllerBase
{
    /// <summary>
    ///     Gets all Audits
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<Trail>>> GetAllAuditsAsync()
    {
        var trails =await  sender.Send(new GetAuditsQueryAsync());
        return Ok(trails);
    }

    /// <summary>
    ///     Gets an audit by id
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType<Trail>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Trail>> GetAuditAsync(string id)
    {
        var trail = await sender.Send(new GetAuditQueryAsync(id));
        return Ok(trail);
    }
}