using Models.DTO;

namespace Models.Interfaces;

public interface IConcert
{
    Task<List<ConcertArtistDetailDto>?> GetConcertsAsync(CancellationToken ct);
    Task<ConcertsGetResponse> GetConcertsAsync(
        int page, int elements, OrderingDirection direction,
        string? orderBy, CancellationToken ct);
    Task<ConcertDto?> GetConcertDtoAsync(int id, CancellationToken ct);
    Task<ConcertArtistDetailDto?> GetConcertAsync(int id, CancellationToken ct);
    // Task<int> AddConcertAsync(ConcertDtoBase concert, CancellationToken ct);
    Task DeleteConcertAsync(int id, CancellationToken ct);
    Task UpdateConcertAsync(ConcertDto updatedConcert, CancellationToken ct);
}

public enum OrderingDirection
{
    Ascending,
    Descending
}