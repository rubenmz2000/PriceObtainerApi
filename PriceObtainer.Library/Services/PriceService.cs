using PriceObtainer.Library.Abstractions;
using PriceObtainer.Library.Models;

namespace PriceObtainer.Library.Services;

public class PriceService
{
    private readonly IPriceRepository _repo;
    public PriceService(IPriceRepository repo) => _repo = repo;

    public Task<PriceSample?> GetPriceAsync(string symbol, CancellationToken ct = default) =>
        _repo.GetLatestAsync(symbol, ct);
    
    public void AddPrice(PriceSample sample) => _repo.AddAsync(sample);
}