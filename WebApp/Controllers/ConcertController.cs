using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO;
using Models.Interfaces;

namespace WebApp.Controllers;

[ApiController]
[Route("concerts")]
public class ConcertController : ControllerBase
{
    private readonly IConcert _concertServices;
    private const int NumberElementsPerPage = 2;
    private readonly ILogger<ConcertController> _logger;

    public ConcertController(ILogger<ConcertController> logger, IConcert concertServices)
    {
        _logger = logger;
        _concertServices = concertServices;
    }

    [HttpGet]
    public async Task<OkObjectResult> GetConcerts()
    {
        return Ok(await _concertServices.GetConcertsAsync());
    }

    [HttpGet("page")]
    public async Task<OkObjectResult> GetConcertsWithPagination(
        int page = 1,
        string? orderBy = "Date",
        OrderingDirection orderingDirection = OrderingDirection.Ascending)
    {
        return Ok(await _concertServices.GetConcertsAsync(page, NumberElementsPerPage, orderingDirection, orderBy));
    }

    [HttpGet("{id:int}")]
    public async Task<IResult> GetConcert(int id)
    {
        var concert = await _concertServices.GetConcertAsync(id);
        return concert is null ? Results.NotFound() : Results.Ok(concert);
    }

    [HttpDelete("{id:int}")]
    public async Task<IResult> DeleteConcert(int id)
    {
        if (await _concertServices.GetConcertAsync(id) is null) return Results.NotFound();
        await _concertServices.DeleteConcertAsync(id);
        return Results.Ok();
    }

    [HttpPut("{id:int}")]
    public async Task<IResult> PutConcert(int id, ConcertDto updatedConcert)
    {
        if (id != updatedConcert.Id) return Results.BadRequest();

        var concert = await _concertServices.GetConcertDtoAsync(id);
        if (concert is null) return Results.NotFound();
        if (updatedConcert.ArtistId != 0) concert.ArtistId = updatedConcert.ArtistId;
        if (updatedConcert.Date.Year != 1) concert.Date = updatedConcert.Date;
        concert.Location = updatedConcert.Location;

        await _concertServices.UpdateConcertAsync(concert);
        return Results.NoContent();
    }

    [HttpPatch("{id:int}")]
    public async Task<IResult> PatchConcert(int id, Concert updatedConcert)
    {
        if (id != updatedConcert.Id) return Results.BadRequest();

        var concert = await _concertServices.GetConcertDtoAsync(id);
        if (concert is null) return Results.NotFound();
        if (updatedConcert.ArtistId != 0) concert.ArtistId = updatedConcert.ArtistId;
        if (updatedConcert.Date.Year != 1) concert.Date = updatedConcert.Date;
        concert.Location = updatedConcert.Location;

        await _concertServices.UpdateConcertAsync(concert);
        return Results.NoContent();
    }
}