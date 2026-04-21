using System.ComponentModel.DataAnnotations;

using AlzaApi.BL.Interfaces;
using AlzaApi.Common.DTOs.Product;
using AlzaApi.Common.Models;

using Microsoft.AspNetCore.Mvc;

namespace AlzaApi.App.Controllers.V2;

/// <summary>
/// Manages product resources (v2).
/// </summary>
[ApiController]
[Route("api/v2/products")]
[ApiExplorerSettings(GroupName = "v2")]
public class ProductsController(IProductService productService) : ControllerBase
{
    /// <summary>
    /// Retrieves a paginated list of products.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve (default: 1).</param>
    /// <param name="pageSize">The number of products per page (default: 10).</param>
    /// <returns>A paginated list of products.</returns>
    /// <response code="200">Products retrieved successfully.</response>
    /// <response code="400">Invalid pagination parameters.</response>
    [ProducesResponseType(typeof(PaginatedResult<ProductListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet]
    public async Task<ActionResult<PaginatedResult<ProductListDto>>> GetPaginated(
        [FromQuery] [Range(1, int.MaxValue, ErrorMessage = "pageNumber must be greater than 0.")]
        int pageNumber = 1,
        [FromQuery] [Range(1, 100, ErrorMessage = "pageSize must be between 1 and 100.")]
        int pageSize = 10)
    {
        var result = await productService.GetAllPaginatedAsync(pageNumber, pageSize);
        return Ok(result);
    }
}
