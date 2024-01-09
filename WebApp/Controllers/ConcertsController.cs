using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO;
using Models.Interfaces;

namespace WebApp.Controllers;

[ApiController]
[Route("[controller]")]
public class ConcertsController : ControllerBase
{
    private readonly IConcert _concertServices;
    private const int NumberElementsPerPage = 2;
    private readonly ILogger<ConcertsController> _logger;

    public ConcertsController(ILogger<ConcertsController> logger, IConcert concertServices)
    {
        _logger = logger;
        _concertServices = concertServices;
    }

    [HttpGet]
    public async Task<IActionResult> GetConcerts()
    {
        var ct = HttpContext.RequestAborted;
        return Ok(await _concertServices.GetConcertsAsync(ct));
    }

    [HttpGet("page")]
    public async Task<IActionResult> GetConcertsWithPagination(
        int page = 1,
        string? orderBy = "Date",
        OrderingDirection orderingDirection = OrderingDirection.Ascending)
    {
        var ct = HttpContext.RequestAborted;
        return Ok(await _concertServices.GetConcertsAsync(page, NumberElementsPerPage, orderingDirection, orderBy, ct));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetConcert(int id)
    {
        var ct = HttpContext.RequestAborted;
        var concert = await _concertServices.GetConcertAsync(id, ct);
        return concert is null ? NotFound() : Ok(concert);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteConcert(int id)
    {
        var ct = HttpContext.RequestAborted;
        if (await _concertServices.GetConcertAsync(id, ct) is null) return NotFound();
        await _concertServices.DeleteConcertAsync(id, ct);
        return Ok();
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutConcert(int id, ConcertDto updatedConcert)
    {
        if (id != updatedConcert.Id) return BadRequest();

        var ct = HttpContext.RequestAborted;
        var concert = await _concertServices.GetConcertDtoAsync(id, ct);
        if (concert is null) return NotFound();
        if (updatedConcert.ArtistId != 0) concert.ArtistId = updatedConcert.ArtistId;
        if (updatedConcert.Date.Year != 1) concert.Date = updatedConcert.Date;
        concert.Location = updatedConcert.Location;

        await _concertServices.UpdateConcertAsync(concert, ct);
        return NoContent();
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> PatchConcert(int id, Concert updatedConcert)
    {
        if (id != updatedConcert.Id) return BadRequest();

        var ct = HttpContext.RequestAborted;
        var concert = await _concertServices.GetConcertDtoAsync(id, ct);
        if (concert is null) return NotFound();
        if (updatedConcert.ArtistId != 0) concert.ArtistId = updatedConcert.ArtistId;
        if (updatedConcert.Date.Year != 1) concert.Date = updatedConcert.Date;
        concert.Location = updatedConcert.Location;

        await _concertServices.UpdateConcertAsync(concert, ct);
        return NoContent();
    }
}