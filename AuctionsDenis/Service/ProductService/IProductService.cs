using AuctionsProject.Models.Products;

namespace AuctionsDenis.Service.ProductService;

public interface IProductService
{
    IEnumerable<GetProduct> GetAllProducts();
    OpenProduct GetProductById(int id);
    void CreateProduct(int userId, CreateProducts model);
    void ExpiredProduct();
    void Bid(Bid model);
    void UpdateProduct(int id, UpdateProduct model);
    void DeleteProduct(int id);
}