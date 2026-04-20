using AlzaApi.BL.Interfaces;
using AlzaApi.Common.DTOs.Product;
using AlzaApi.Common.Models;

using Microsoft.AspNetCore.Mvc;

namespace AlzaApi.App.Controllers.V2;

[ApiController]
[Route("api/v2/[controller]")]
[ApiExplorerSettings(GroupName = "v2")]
public class ProductsController(IProductService productService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginatedResult<ProductListDto>>> GetPaginated(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await productService.GetAllPaginatedAsync(pageNumber, pageSize);
        return Ok(result);
    }
}
