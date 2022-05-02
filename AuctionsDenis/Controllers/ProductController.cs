using AuctionsDenis.Service.ProductService;
using AuctionsProject.Models.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApi.Authorization;
using WebApi.Helpers;
using WebApi.Models.Users;

namespace AuctionsDenis.Controllers;

[Authorize]
[ApiController]
[Route("[controller]/")]
public class ProductController : ControllerBase
{
private IProductService _productService;
private readonly AppSettings _appSettings;

public ProductController(
    IProductService productService,
    IOptions<AppSettings> appSettings)
{
    _productService = productService;
    _appSettings = appSettings.Value;
}
/// <summary>
/// to create a new product
/// </summary>
/// <param name="userId">id of the user that is creating the product</param>
/// <param name="model"></param>
/// <returns></returns>
[HttpPost("CreateProduct{userId}")]
public IActionResult CreateProduct(int userId,CreateProducts model)
{
    _productService.CreateProduct(userId, model);
    return Ok(new { message = "Product Created successful" });
}
/// <summary>
/// to meke a bid on a product 
/// </summary>
/// <param name="bid"></param>
/// <returns></returns>
[HttpPost("Bid")]
public IActionResult Bid([FromBody]Bid bid)
{
    _productService.Bid(bid);
    return Ok(new { message = "Bid placed successful" });
}
/// <summary>
/// returns all active products
/// </summary>
/// <returns></returns>
[HttpGet("AllProducts")]
public IActionResult GetAll()
{
    var products = _productService.GetAllProducts();
    return Ok(products);
}
/// <summary>
/// to open a product by id
/// </summary>
/// <param name="id"></param>
/// <returns></returns>
[HttpGet("product/{id}")]
public IActionResult GetById(int id)
{
    var product = _productService.GetProductById(id);
    return Ok(product);
}
/// <summary>
/// to update a product name or finishing date
/// </summary>
/// <param name="id"></param>
/// <param name="model"></param>
/// <returns></returns>
[HttpPut("product/{id}")]
public IActionResult Update(int id, UpdateProduct model)
{
    _productService.UpdateProduct(id, model);
    return Ok(new { message = "Product updated successfully" });
}
/// <summary>
/// To Delete a product
/// </summary>
/// <param name="id"></param>
/// <returns></returns>
[HttpDelete("product/{id}")]
public IActionResult Delete(int id)
{
    _productService.DeleteProduct(id);
    return Ok(new { message = "Product deleted successfully" });
}

}
