using Database;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTO;
using Models.Interfaces;
using Services.DTO;

namespace Services;

public class DbArtistsImpl(MusicDbContext context) : IArtist
{
    public async Task<List<ArtistConcertsDetailWithIdDto>?> GetArtistsAsync(CancellationToken ct)
    {
        return (await context.Artists
                .AsNoTracking()
                .Include(artist => artist.Concerts)
                .ToListAsync(cancellationToken: ct))
            .ConvertArtistsToDto();
    }

    public async Task<ArtistDtoWithId> AddArtistAsync(ArtistDtoEssential newArtist, CancellationToken ct)
    {
        var artistDb = newArtist.ConvertDtoToArtist();

        await context.Artists.AddAsync(artistDb, ct);
        await context.SaveChangesAsync(ct);

        return artistDb.ConvertArtistToDtoWithId();
    }

    public async Task<bool> AddConcertToArtist(int id, ConcertDtoEssential newConcert, CancellationToken ct)
    {
        var artistDb = await context.Artists.FirstOrDefaultAsync(artist => artist.Id == id, cancellationToken: ct);
        if (artistDb == null)
            return false;
        var newConcertDb = new Concert()
        {
            Date = newConcert.Date,
            Location = newConcert.Location,
            ArtistId = id
        };
        await context.Concerts.AddAsync(newConcertDb, ct);
        await context.SaveChangesAsync(ct);
        return true;
    }

    public async Task AddArtistWithConcertsAsync(ArtistConcertsDetailDto newArtist, CancellationToken ct)
    {
        var artistDb = new Artist()
        {
            Name = newArtist.Name,
            Surname = newArtist.Surname,
            BirthYear = newArtist.BirthYear,
            Concerts = newArtist.Concerts.Select(c => new Concert
            {
                Location = c.Location,
                Date = c.Date
            }).ToList()
        };
        await context.Artists.AddAsync(artistDb, ct);
        await context.SaveChangesAsync(ct);
    }
}