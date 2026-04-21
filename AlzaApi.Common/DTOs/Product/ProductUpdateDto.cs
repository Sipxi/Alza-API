using System.ComponentModel.DataAnnotations;

namespace AlzaApi.Common.DTOs.Product;

/// <summary>
/// Payload for updating a product's description.
/// </summary>
public class ProductUpdateDto
{
    /// <summary>
    /// The new description to assign to the product. Maximum 1000 characters.
    /// </summary>
    [MaxLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
    public string Description { get; set; } = string.Empty;
}
