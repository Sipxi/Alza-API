using System.ComponentModel.DataAnnotations;

namespace AlzaApi.Common.DTOs.Product;

public class ProductUpdateDto
{
    [MaxLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
    public string Description { get; set; } = string.Empty;
}
