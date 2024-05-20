using eShopMassTransit.Models;
using eShopMassTransit.Service;
using Microsoft.AspNetCore.Mvc;

namespace eShopMassTransit.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }


    [HttpPost]
    public async Task<IActionResult> CreateProduct(Product product)
    {
        var result = await _productService.AddProduct(product);

        return Ok(result);
    }
}