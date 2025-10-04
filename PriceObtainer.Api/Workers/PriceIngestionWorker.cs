using PriceObtainer.Library.Models;
using PriceObtainer.Library.Services;

namespace PriceObtainer.Api.Workers;

public class PriceIngestionWorker : BackgroundService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IServiceProvider _serviceProvider;
    
    private static readonly string[] Symbols = { "BTCUSDT" };
    private const int IntervalSeconds = 5;

    public PriceIngestionWorker(IHttpClientFactory httpClientFactory, IServiceProvider serviceProvider)
    {
        _httpClientFactory = httpClientFactory;
        _serviceProvider = serviceProvider;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var http = _httpClientFactory.CreateClient("binance");
        while (!stoppingToken.IsCancellationRequested)
        {
            foreach (var symbol in Symbols)
            {
                try
                {
                    var url = $"/api/v3/ticker/price?symbol={symbol}";
                    var resp = await http.GetFromJsonAsync<BinanceTickerPrice>(url);
                    if (resp is null || !decimal.TryParse(resp.price, System.Globalization.NumberStyles.Any,
                            System.Globalization.CultureInfo.InvariantCulture, out var price))
                        continue;

                    Console.WriteLine($"[{DateTime.UtcNow:O}] Ingest {symbol}: {price}");
                    using var scope = _serviceProvider.CreateScope();
                    var priceService = scope.ServiceProvider.GetRequiredService<PriceService>();

                    var sample = new PriceSample()
                    {
                        Symbol = symbol,
                        Timestamp = DateTime.UtcNow,
                        Price = price
                    };

                    priceService.AddPrice(sample);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[{DateTime.UtcNow:O}] Ingest error {symbol}: {ex.Message}");
                }
            }
            
            await Task.Delay(TimeSpan.FromSeconds(IntervalSeconds), stoppingToken);
        }
    }

    private record BinanceTickerPrice(string Symbol, string price);
}