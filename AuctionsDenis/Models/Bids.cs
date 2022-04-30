using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionsProject.Models;

public class Bids
{
    [Key]
    public int BidId { get; set; }
    public int ProductId { get; set; }
    public int BiderId { get; set; }
    [Column(TypeName = "decimal(8,2)")]
    public decimal BidAmount { get; set; }
   
}