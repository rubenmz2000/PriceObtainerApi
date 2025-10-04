namespace PriceObtainer.Library.Abstractions;
using PriceObtainer.Library.Models;

public interface IPriceRepository
{
    Task<PriceSample?> GetLatestAsync(string symbol, CancellationToken ct = default);
    Task AddAsync(PriceSample sample, CancellationToken ct = default);
}