using AuctionsDenis.Service.UserService;
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
    private readonly IUserService _userService;

    public ProductService(
        AuctionsContext context,
        IMapper mapper,IUserService userService)
    {
        _context = context;
        _mapper = mapper;
        _userService = userService;
    }

    public  IEnumerable<GetProduct> GetAllProducts()
    {
        var query = (from p in _context.Set<Product>()
            where p.StatusActive==true
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

    public OpenProduct GetProductById(int id)
    {
        var item = new OpenProduct();
        var product = (from p in _context.Set<Product>()
            join b in _context.Set<Bids>()
                on p.HighestBidId equals b.BidId into bids
            from b in bids.DefaultIfEmpty()
            join u in _context.Set<Users>() on p.SellerId equals u.UserId 
            where p.ProductId==id
            select  new GetProduct()
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                SellerName = u.UserName,
                StartingBid = p.StartingBid,
                TimeLeft = p.FinishingDate - DateTime.Now,
                HighestBid = b.BidAmount
                   
            }).FirstOrDefault();
        item.Product = product;
        item.Bids = GetBidsByProduct(id);
        return item;
    }

    public void ExpiredProduct()
    {
        var product = (from p in _context.Set<Product>()
            where p.FinishingDate < DateTime.Now && p.StatusActive == true 
                select p).ToList();

        foreach (var p in product)
        {
            if (p.HighestBidId!=0)
            {
                var bid = GetBid(p.HighestBidId);
                var biderWallet= _userService.GetWallet(bid.BiderId);
                var sellerWallet= _userService.GetWallet(p.SellerId);
                biderWallet.Amount = biderWallet.Amount - bid.BidAmount;
                sellerWallet.Amount = sellerWallet.Amount + bid.BidAmount;
                sellerWallet.UsableAmount = sellerWallet.UsableAmount + bid.BidAmount;
                _context.Wallet.Update(biderWallet);
                _context.Wallet.Update(sellerWallet);
                _context.SaveChanges();
            }
            var item = GetProduct(p.ProductId);
            item.StatusActive = false;
            _context.Product.Update(item);
            _context.SaveChanges();
        }
        Console.WriteLine("done : " + DateTime.Now.ToString());
        
    }

    public void Bid(Bid model)
    {
        var wallet = _userService.GetWallet(model.UserId);
        var item = GetProduct(model.ProductId);
        var product = (from p in _context.Set<Product>()
            join b in _context.Set<Bids>()
                on p.HighestBidId equals b.BidId into bids
            from b in bids.DefaultIfEmpty()
            where p.ProductId==model.ProductId
            select  new GetProduct()
            {
                ProductId = p.ProductId,
                StartingBid = p.StartingBid,
                TimeLeft = p.FinishingDate - DateTime.Now,
                HighestBid = b.BidAmount
                   
            }).FirstOrDefault();
        if (product is not null)
        {
            ValidateBid(model, product, wallet);
            var bid = model.Adapt<Bids>();
            _context.Bids.Add(bid);
            _context.SaveChanges();
            var oldBid = GetBid(item.HighestBidId);
            var oldBiderWallet= _userService.GetWallet(oldBid.BiderId);
            oldBiderWallet.UsableAmount = oldBiderWallet.UsableAmount + oldBid.BidAmount;
            item.HighestBidId = bid.BidId;
            wallet.UsableAmount = wallet.UsableAmount - model.Amount;
            _context.Product.Update(item);
            _context.Wallet.Update(oldBiderWallet);
            _context.Wallet.Update(wallet);
            _context.SaveChanges();
            
        }
    }

    private static void ValidateBid(Bid model, GetProduct product, Wallet wallet)
    {
        if (((product.HighestBid is null || product.HighestBid == 0) && model.Amount < product.StartingBid) ||
            (product.HighestBid is not null && product.HighestBid != 0 && model.Amount <= product.HighestBid) ||
            product.TimeLeft.TotalSeconds < 0 || wallet.UsableAmount < model.Amount)
        {
            throw new KeyNotFoundException("Bid did not take place");
        }
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
        if (product.HighestBidId !=0)
        {
            var oldBid = GetBid(product.HighestBidId);
            var oldBiderWallet= _userService.GetWallet(oldBid.BiderId);
            oldBiderWallet.UsableAmount = oldBiderWallet.UsableAmount + oldBid.BidAmount;
            _context.Wallet.Update(oldBiderWallet);
        }
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
    private List<Bids> GetBidsByProduct(int productId)
    {
        var bids = (from b in _context.Set<Bids>()
            where b.ProductId == productId
            select b);

        return bids.ToList();
    }
    private Bids GetBid(int id)
    {
        var bids = _context.Bids.Find(id);
        if (bids == null) throw new KeyNotFoundException("Product not found");
        return bids;
    }
   
}