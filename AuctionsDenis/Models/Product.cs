using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionsProject.Models;

public class Product
{
    [Key]
    public int ProductId { get; set; }
    public int SellerId { get; set; }
    [Required]
    public string ProductName { get; set; }
    [Column(TypeName = "decimal(8,2)")]
    public decimal StartingBid { get; set; }
    public bool StatusActive { get; set; }
    public DateTime StartingDate { get; set; }
    public DateTime FinishingDate { get; set; }
    public int HighestBidId { get; set; }
    public ICollection<Bids> Bids { get; set; }
}