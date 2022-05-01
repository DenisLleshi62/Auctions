using Humanizer;
using Humanizer.Localisation;

namespace AuctionsProject.Models.Products;

public class GetProduct
{
    public int ProductId { get; set; }
    public string SellerName { get; set; }
    public string ProductName { get; set; }
    public decimal StartingBid { get; set; }
    internal  TimeSpan TimeLeft { get; set; }
    public string TimeRemaining => TimeLeft.Humanize(minUnit: TimeUnit.Minute);
    public decimal? HighestBid { get; set; }
}