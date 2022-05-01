using AuctionsDenis.Service.ProductService;
using AuctionsProject.Models.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApi.Authorization;
using WebApi.Helpers;
using WebApi.Models.Users;

namespace AuctionsDenis.Controllers;

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

[AllowAnonymous]
[HttpPost("CreateProduct{userId}")]
public IActionResult CreateProduct(int userId,CreateProducts model)
{
    _productService.CreateProduct(userId, model);
    return Ok(new { message = "Product Created successful" });
}
[AllowAnonymous]
[HttpPost("Bid")]
public IActionResult Bid([FromBody]Bid bid)
{
    _productService.Bid(bid);
    return Ok(new { message = "Bid placed successful" });
}
[AllowAnonymous]
[HttpGet("AllProducts")]
public IActionResult GetAll()
{
    var products = _productService.GetAllProducts();
    return Ok(products);
}
[AllowAnonymous]
[HttpGet("product/{id}")]
public IActionResult GetById(int id)
{
    var product = _productService.GetProductById(id);
    return Ok(product);
}
[AllowAnonymous]
[HttpPut("product/{id}")]
public IActionResult Update(int id, UpdateProduct model)
{
    _productService.UpdateProduct(id, model);
    return Ok(new { message = "Product updated successfully" });
}
[AllowAnonymous]
[HttpDelete("product/{id}")]
public IActionResult Delete(int id)
{
    _productService.DeleteProduct(id);
    return Ok(new { message = "Product deleted successfully" });
}

}
