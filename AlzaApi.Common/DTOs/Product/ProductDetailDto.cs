namespace AlzaApi.Common.DTOs.Product;

/// <summary>
/// Represents detailed information about a product.
/// </summary>
public class ProductDetailDto
{
    /// <summary>The unique identifier of the product.</summary>
    public Guid Id { get; set; }

    /// <summary>The name of the product.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>The description of the product.</summary>
    public string? Description { get; set; }

    /// <summary>The price of the product.</summary>
    public decimal Price { get; set; }

    /// <summary>The URL of the product image.</summary>
    public string ImageUrl { get; set; } = string.Empty;
}
