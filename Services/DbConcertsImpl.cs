using Database;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTO;
using Models.Interfaces;
using Services.DTO;

namespace Services;

public class DbConcertsImpl(MusicDbContext context) : IConcert
{
    public async Task<List<ConcertArtistDetailDto>?> GetConcertsAsync(CancellationToken ct)
    {
        var concertDb = await context.Concerts
            .AsNoTracking()
            .Include(concert => concert.Artist)
            .ToListAsync(cancellationToken: ct);
        return concertDb.ConvertConcertsToDto();
    }

    public async Task<ConcertsGetResponse> GetConcertsAsync(
        int page, int elements, OrderingDirection direction,
        string? orderBy, CancellationToken ct)
    {
        var response = new ConcertsGetResponse
        {
            Total = await context.Concerts
                .AsNoTracking()
                .CountAsync(cancellationToken: ct),
            Page = page
        };
        var selectedConcerts = context.Concerts
            .AsNoTracking()
            .Include(concert => concert.Artist);
        var orderedConcerts = orderBy?.ToLower() switch
        {
            "location" => direction == OrderingDirection.Ascending
                ? selectedConcerts.OrderBy(concert => concert.Location)
                : selectedConcerts.OrderByDescending(concert => concert.Location),
            _ => direction == OrderingDirection.Ascending
                ? selectedConcerts.OrderBy(concert => concert.Date)
                : selectedConcerts.OrderByDescending(concert => concert.Date)
        };
        var concertsList = await orderedConcerts
            .Skip(elements * (page - 1))
            .Take(elements)
            .ToListAsync(cancellationToken: ct);
        response.Concerts = concertsList.ConvertConcertsToDto();

        return response;
    }

    public async Task<ConcertDto?> GetConcertDtoAsync(int id, CancellationToken ct)
    {
        var concertDb = await context.Concerts
            .AsNoTracking()
            .Include(concert => concert.Artist)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken: ct);
        return concertDb?.ConvertConcertSpecialToDto();
    }

    public async Task<ConcertArtistDetailDto?> GetConcertAsync(int id, CancellationToken ct)
    {
        var concertDb = await context.Concerts
            .AsNoTracking()
            .Include(concert => concert.Artist)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken: ct);
        return concertDb?.ConvertConcertToDto();
    }

    public async Task<int> AddConcertAsync(ConcertDtoBase concert, CancellationToken ct)
    {
        var concertDb = concert.ConvertDtoToConcert();

        if (concert.ArtistId == 0 && concert.Artist != null!)
        {
            var newArtist = new Artist
            {
                Name = concert.Artist.Name,
                Surname = concert.Artist.Surname,
                BirthYear = concert.Artist.BirthYear
            };
            await context.Artists.AddAsync(newArtist, ct);
            await context.SaveChangesAsync(ct);
            concertDb.ArtistId = newArtist.Id;
        }

        await context.Concerts.AddAsync(concertDb, ct);
        await context.SaveChangesAsync(ct);
        return concertDb.Id;
    }

    public async Task DeleteConcertAsync(int id, CancellationToken ct)
    {
        var concertDb = await context.Concerts
            .AsNoTracking()
            .Include(concert => concert.Artist)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken: ct);
        if (concertDb is not null)
        {
            context.Artists.RemoveRange(concertDb.Artist);
            context.Concerts.Remove(concertDb);
        }

        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateConcertAsync(ConcertDto updatedConcert, CancellationToken ct)
    {
        var concertDb = await context.Concerts
            .FirstOrDefaultAsync(x => x.Id == updatedConcert.Id, cancellationToken: ct);

        if (concertDb is not null)
        {
            concertDb.Date = updatedConcert.Date;
            concertDb.Location = updatedConcert.Location;
        }

        await context.SaveChangesAsync(ct);
    }
}