using Microsoft.AspNetCore.Mvc;
using Models.DTO;
using Models.Interfaces;

namespace WebApp.Controllers;

[ApiController]
[Route("artists")]
public class ArtistController : ControllerBase
{
    private readonly IArtist _artistServices;
    private readonly ILogger<ArtistController> _logger;

    public ArtistController(ILogger<ArtistController> logger, IArtist artistServices)
    {
        _logger = logger;
        _artistServices = artistServices;
    }

    [HttpGet]
    public async Task<OkObjectResult> GetArtists()
    {
        var ct = HttpContext.RequestAborted;
        var artists = await _artistServices.GetArtistsAsync(ct);
        return Ok(artists);
    }

    [HttpPost]
    public async Task<IResult> PostArtist(ArtistDtoEssential newArtist)
    {
        var ct = HttpContext.RequestAborted;
        var newArtistWithId = await _artistServices.AddArtistAsync(newArtist, ct);
        return Results.Created($"artists/{newArtistWithId.Id}", newArtistWithId);
    }

    [HttpPost("{id:int}/concerts")]
    public async Task<IResult> PostConcertsToArtist(int id, ConcertDtoEssential newConcerts)
    {
        var ct = HttpContext.RequestAborted;
        if (await _artistServices.AddConcertToArtist(id, newConcerts, ct))
        {
            return Results.Ok();
        }

        return Results.NotFound();
    }

    [HttpPost("concerts")]
    public async Task PostArtistWithConcerts(ArtistConcertsDetailDto newArtist)
    {
        var ct = HttpContext.RequestAborted;
        await _artistServices.AddArtistWithConcertsAsync(newArtist, ct);
    }
}