using Microsoft.EntityFrameworkCore;
using PriceObtainer.DataAccess.Context;
using PriceObtainer.Library.Abstractions;
using PriceObtainer.Library.Models;

namespace PriceObtainer.DataAccess.Repositories;

public class PriceRepository : IPriceRepository
{
    private readonly PriceObtainerDbContext _db;
    public PriceRepository(PriceObtainerDbContext db) => _db = db;

    public Task<PriceSample?> GetLatestAsync(string symbol, CancellationToken ct = default) =>
        _db.Prices.Where(p => p.Symbol == symbol)
            .OrderByDescending(p => p.Timestamp)
            .FirstOrDefaultAsync(ct);
    

    public async Task AddAsync(PriceSample sample, CancellationToken ct = default)
    {
        await _db.Prices.AddAsync(sample, ct);
        await _db.SaveChangesAsync(ct);
    }
}