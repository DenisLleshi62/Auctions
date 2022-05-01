namespace AuctionsProject.Models.Products;

public class Bid
{
   public int ProductId{get;set;}
   public int UserId{get;set;}
   public decimal Amount{get;set;}
}