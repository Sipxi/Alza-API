using System.ComponentModel.DataAnnotations;

namespace AlzaApi.DAL.Entities
{
    public class Product : BaseEntity
    {
        [StringLength(50)]
        public required string Name { get; set; }

        [StringLength(1000)]
        public required string ImgUri { get; set; }

        public required Decimal Price { get; set; }

        [StringLength(2048)]
        public string? Description { get; set; }
    }
}
