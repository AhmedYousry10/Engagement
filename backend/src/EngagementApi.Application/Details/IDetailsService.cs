namespace EngagementApi.Application.Details;

public interface IDetailsService
{
    Task<List<DetailCardDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<DetailCardDto> CreateAsync(DetailCardUpsertDto dto, CancellationToken cancellationToken = default);
    Task<DetailCardDto?> UpdateAsync(int id, DetailCardUpsertDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
