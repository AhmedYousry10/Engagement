using EngagementApi.Application.Common.Interfaces;
using EngagementApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EngagementApi.Application.Details;

public class DetailsService : IDetailsService
{
    private readonly IAppDbContext _db;

    public DetailsService(IAppDbContext db)
    {
        _db = db;
    }

    public async Task<List<DetailCardDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _db.DetailCards
            .OrderBy(d => d.SortOrder)
            .Select(d => ToDto(d))
            .ToListAsync(cancellationToken);
    }

    public async Task<DetailCardDto> CreateAsync(DetailCardUpsertDto dto, CancellationToken cancellationToken = default)
    {
        var card = new DetailCard { Title = dto.Title, Body = dto.Body, SortOrder = dto.SortOrder };
        _db.DetailCards.Add(card);
        await _db.SaveChangesAsync(cancellationToken);
        return ToDto(card);
    }

    public async Task<DetailCardDto?> UpdateAsync(int id, DetailCardUpsertDto dto, CancellationToken cancellationToken = default)
    {
        var card = await _db.DetailCards.FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
        if (card is null)
        {
            return null;
        }

        card.Title = dto.Title;
        card.Body = dto.Body;
        card.SortOrder = dto.SortOrder;
        await _db.SaveChangesAsync(cancellationToken);

        return ToDto(card);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var card = await _db.DetailCards.FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
        if (card is null)
        {
            return false;
        }

        _db.DetailCards.Remove(card);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static DetailCardDto ToDto(DetailCard card) => new()
    {
        Id = card.Id,
        Title = card.Title,
        Body = card.Body,
        SortOrder = card.SortOrder
    };
}
