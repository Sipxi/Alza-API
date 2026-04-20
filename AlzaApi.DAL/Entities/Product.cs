namespace AlzaApi.DAL.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = null!;

        public string ImgUri { get; set; } = null!;

        public Decimal Price { get; set; }

        public string? Description { get; set; }
    }
}
