using AlzaApi.BL.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace AlzaApi.App.Controllers.V1;

[ApiController]
[Route("api/v1/[controller]")]
[ApiExplorerSettings(GroupName = "v1")] // Подсказка для Swagger, чтобы он не путался
public class ProductController(IProductService productService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await productService.GetAllAsync();
        return Ok(products);
    }

    [HttpGet("{id:guid}/")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var product = await productService.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    [HttpPut("{id:guid}/description")]
    public async Task<IActionResult> UpdateDescription(Guid id, [FromBody] string description)
    {
        var success = await productService.UpdateDescriptionAsync(id, description);
        if (!success) return NotFound();
        return NoContent();
    }
}
