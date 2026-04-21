using AlzaApi.BL.Interfaces;
using AlzaApi.Common.DTOs.Product;

using Microsoft.AspNetCore.Mvc;

namespace AlzaApi.App.Controllers.V1;

/// <summary>
/// Manages product resources.
/// </summary>
[ApiController]
[Route("api/v1/products")]
[ApiExplorerSettings(GroupName = "v1")]
public class ProductController(IProductService productService) : ControllerBase
{
    /// <summary>
    /// Retrieves all products.
    /// </summary>
    /// <returns>A list of all available products.</returns>
    /// <response code="200">Products retrieved successfully.</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<ProductListDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ProductListDto>>> GetProducts()
    {
        var products = await productService.GetAllAsync();
        return Ok(products);
    }

    /// <summary>
    /// Retrieves a product by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the product.</param>
    /// <returns>The product with the specified ID.</returns>
    /// <response code="200">Product retrieved successfully.</response>
    /// <response code="404">Product with the specified ID was not found.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProductDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductDetailDto>> GetById(Guid id)
    {
        var product = await productService.GetByIdAsync(id);
        if (product == null) return NotFound();
        return Ok(product);
    }

    /// <summary>
    /// Updates the description of a product.
    /// </summary>
    /// <param name="id">The unique identifier of the product.</param>
    /// <param name="updateDto">The new description for the product.</param>
    /// <response code="204">Description updated successfully.</response>
    /// <response code="404">Product with the specified ID was not found.</response>
    /// <response code="400">Description is null or invalid.</response>
    [HttpPatch("{id:guid}/description")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateDescription(Guid id, [FromBody] ProductUpdateDto updateDto)
    {
        var success = await productService.UpdateDescriptionAsync(id, updateDto.Description);
        if (!success) return NotFound();
        return NoContent();
    }
}
