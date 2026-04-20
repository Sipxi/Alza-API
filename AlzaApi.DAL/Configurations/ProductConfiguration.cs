using AlzaApi.DAL.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlzaApi.DAL.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // Table name
        builder.ToTable("Products");

        // Primary key
        builder.HasKey(p => p.Id);

        // Name rules
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(50);

        // Price rules
        builder.Property(p => p.Price)
            .IsRequired()
            .HasPrecision(10, 2);

        // Image
        builder.Property(p => p.ImgUri)
            .IsRequired()
            .HasMaxLength(2048);

        // Description
        builder.Property(p => p.Description)
            .HasMaxLength(1000);
        
        // Creating, in bigger systems can centralize this
        builder.Property(p => p.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");
    }
}
