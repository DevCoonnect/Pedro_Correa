using AuthMuseum.Core.Helpers;
using AuthMuseum.Core.Requirements;
using AuthMuseum.Core.Services;
using AuthMuseum.Domain.Enums;
using AuthMuseum.Domain.Requests;
using Microsoft.AspNetCore.Mvc;

namespace AuthMuseum.Application.Controllers;

[ApiController]
[Route("api")]

/*
 * Will need permission:
 * Create Art
 * Delete Art
 */
public class ArtController(IArtService artService) : ControllerBase
{
    
    [HttpPost("art")]
    public async Task<IActionResult> CreateArtAsync([FromBody] CreateArtRequest request)
    {
        var result = await artService.CreateArt(request);

        return result.Match(
            response => CreatedAtAction("GetArtById", new { response.Id }, response),
            error => error.ErrorToActionResult()
        );
    }

    [HttpGet("art/{id:long}")]
    public async Task<IActionResult> GetArtByIdAsync(long id)
    {
        var art = await artService.GetArtById(id);

        return art.Match(
            Ok,
            error => error.ErrorToActionResult()
        );
    }
    
    [HttpGet("arts")]
    [RequirePermissions(Permissions.CAN_RETRIEVE_ARTS)]
    public async Task<IActionResult> ListArtsAsync()
    {
        var arts = await artService.GetArts();

        return arts.Match(
            Ok,
            error => error.ErrorToActionResult()
        );
    }

    [HttpDelete("art/{id:long}")]
    public async Task<IActionResult> DeleteArtAsync(long id)
    {
        var arts = await artService.DeleteArt(id);

        return arts.Match(
            Ok,
            error => error.ErrorToActionResult()
        );
    }
}