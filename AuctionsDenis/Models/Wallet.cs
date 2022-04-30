using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionsProject.Models;

public class Wallet
{
    [Key]
    public int UserId { get; set; }
    [Column(TypeName = "decimal(8,2)")]
    public decimal Amount { get; set; }
    [Column(TypeName = "decimal(8,2)")]
    public decimal UsableAmount { get; set; }
}