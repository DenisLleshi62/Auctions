using Humanizer;
using Humanizer.Localisation;

namespace AuctionsProject.Models.Products;

public class OpenProduct
{
    public GetProduct? Product { get; set; }
    public List<Bids> Bids { get; set;}
}