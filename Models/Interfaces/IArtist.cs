using Models.DTO;

namespace Models.Interfaces;

public interface IArtist
{
    Task<List<ArtistConcertsDetailWithIdDto>?> GetArtistsAsync(CancellationToken ct);
    Task<ArtistDtoWithId> AddArtistAsync(ArtistDtoEssential newArtist, CancellationToken ct);
    Task<bool> AddConcertToArtist(int id, ConcertDtoEssential newConcert, CancellationToken ct);
    Task AddArtistWithConcertsAsync(ArtistConcertsDetailDto newArtist, CancellationToken ct);
}