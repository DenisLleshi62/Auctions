using AuctionsProject.Models.Products;

namespace AuctionsDenis.Service.ProductService;

public interface IProductService
{
    IEnumerable<GetProduct> GetAllProducts();
   // Users GetById(int id);
    void CreateProduct(int userId, CreateProducts model);
    void UpdateProduct(int id, UpdateProduct model);
    void DeleteProduct(int id);
}