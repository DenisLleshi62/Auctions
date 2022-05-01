namespace AuctionsProject.Models.Products;

public class CreateProducts
{
    public string ProductName { get; set; }
    public decimal StartingBid { get; set; }
    public DateTime FinishingDate { get; set; }
}