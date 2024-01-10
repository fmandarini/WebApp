using Microsoft.AspNetCore.Mvc;
using Models.DTO;
using Models.Interfaces;

namespace WebApp.Controllers;

[ApiController]
[Route("[controller]")]
public class ArtistsController : ControllerBase
{
    private readonly IArtist _artistServices;
    private readonly ILogger<ArtistsController> _logger;

    public ArtistsController(ILogger<ArtistsController> logger, IArtist artistServices)
    {
        _logger = logger;
        _artistServices = artistServices;
    }

    [HttpGet]
    public async Task<IActionResult> GetArtists()
    {
        var ct = HttpContext.RequestAborted;
        var artists = await _artistServices.GetArtistsAsync(ct);
        return Ok(artists);
    }

    [HttpPost]
    public async Task<IActionResult> PostArtist(ArtistDtoEssential newArtist)
    {
        var ct = HttpContext.RequestAborted;
        var newArtistWithId = await _artistServices.AddArtistAsync(newArtist, ct);
        return Created($"artists/{newArtistWithId.Id}", newArtistWithId);
    }

    [HttpPost("{id:int}/concerts")]
    public async Task<IActionResult> PostConcertsToArtist(int id, ConcertDtoEssential newConcerts)
    {
        var ct = HttpContext.RequestAborted;
        if (await _artistServices.AddConcertToArtist(id, newConcerts, ct))
        {
            return Ok();
        }

        return NotFound();
    }

    [HttpPost("concerts")]
    public async Task PostArtistWithConcerts(ArtistConcertsDetailDto newArtist)
    {
        var ct = HttpContext.RequestAborted;
        await _artistServices.AddArtistWithConcertsAsync(newArtist, ct);
    }
}