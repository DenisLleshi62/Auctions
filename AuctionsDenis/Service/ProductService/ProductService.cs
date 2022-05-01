using AuctionsProject.Data;
using AuctionsProject.Models;
using AuctionsProject.Models.Products;
using Humanizer;
using Humanizer.Localisation;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore.Internal;

namespace AuctionsDenis.Service.ProductService;

public class ProductService : IProductService
{
    private AuctionsContext _context;
    private readonly IMapper _mapper;

    public ProductService(
        AuctionsContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public  IEnumerable<GetProduct> GetAllProducts()
    {
        var query = (from p in _context.Set<Product>()
            join b in _context.Set<Bids>()
                on p.HighestBidId equals b.BidId into bids
            from b in bids.DefaultIfEmpty()
            join u in _context.Set<Users>() on p.SellerId equals u.UserId 
            select  new GetProduct()
        {
            ProductId = p.ProductId,
            ProductName = p.ProductName,
            SellerName = u.UserName,
            StartingBid = p.StartingBid,
            TimeLeft = p.FinishingDate - DateTime.Now,
            HighestBid = b.BidAmount
                   
        }).ToList().OrderBy(x=>x.TimeLeft.TotalMinutes);;
        return query;
      
    }

    public void UpdateProduct(int id, UpdateProduct model)
    {
        var product = GetProduct(id);

     
        if (string.IsNullOrEmpty(model.ProductName))
            model.ProductName = product.ProductName;
        if (model.FinishingDate >= DateTime.Now)
        {
            throw new KeyNotFoundException("Date not Valid");
        }
    
        _mapper.Map(model, product);
        _context.Product.Update(product);
        _context.SaveChanges();
    }

    public void DeleteProduct(int id)
    {
        var product = GetProduct(id);
        _context.Product.Remove(product);
        _context.SaveChanges();
    }
    public void CreateProduct(int userId,CreateProducts model)
    {
        // map model to new product object
        var product = _mapper.Map<Product>(model);
        product.StartingDate = DateTime.Now;
        product.SellerId = userId;
        product.StatusActive = true;
        _context.Product.Add(product);
        _context.SaveChanges();
        
            
    }
    private Product GetProduct(int id)
    {
        var product = _context.Product.Find(id);
        if (product == null) throw new KeyNotFoundException("Product not found");
        return product;
    }
}