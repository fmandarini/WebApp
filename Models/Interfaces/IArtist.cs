using Models.DTO;

namespace Models.Interfaces;

public interface IArtist
{
    Task<List<ArtistConcertsDetailWithIdDto>?> GetArtistsAsync();
    Task<ArtistDtoWithId> AddArtistAsync(ArtistDtoEssential newArtist);
    Task<bool> AddConcertToArtist(int id, ConcertDtoEssential newConcert);
    Task AddArtistWithConcertsAsync(ArtistConcertsDetailDto newArtist);
}