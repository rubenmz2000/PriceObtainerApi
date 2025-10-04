namespace PriceObtainer.Library.Models;

public class PriceSample
{
    public Guid Id { get; set; }
    public string Symbol { get; set; }
    public DateTime Timestamp { get; set; }
    public decimal Price { get; set; }
}