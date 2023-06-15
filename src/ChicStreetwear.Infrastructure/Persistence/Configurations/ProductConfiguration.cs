using ChicStreetwear.Domain.Aggregates.Category;
using ChicStreetwear.Domain.Aggregates.Product;
using ChicStreetwear.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChicStreetwear.Infrastructure.Persistence.Configurations;
internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    private const string ID = "Id";
    private const string PRODUCT_ID = "ProductId";
    private const string VARIATION_ID = "VariationId";

    public void Configure(EntityTypeBuilder<Product> builder)
    {
        ConfigureProductCategory(builder);
        ConfigureProductPicture(builder);
        ConfigureProductAttributes(builder);
        ConfigureVariations(builder);

        builder.Property(p => p.Name)
               .HasMaxLength(Constants.Fields.NAME_MAXLENGTH);

        builder.Property(p => p.Description)
               .HasMaxLength(Constants.Fields.DESCRIPTION_MAXLENGTH);

        builder.OwnsOne(p => p.PurchasedPrice, ppb =>
        {
            ppb.Property(pp => pp.Amount)
               .HasColumnName("PurchasedPrice")
               .HasColumnType("money");
        });

        builder.OwnsOne(p => p.RegularPrice, rpb =>
        {
            rpb.Property(rp => rp.Amount)
               .HasColumnName("RegularPrice")
               .HasColumnType("money");
        });

        builder.OwnsOne(p => p.SalePrice, spb =>
        {
            spb.Property(sp => sp.Amount)
               .HasColumnName("SalePrice")
               .HasColumnType("money");
        });

        builder.OwnsOne(p => p.CoverPicture, cpb =>
        {
            cpb.Property(cp => cp.FileName)
              .HasColumnName("CoverPictureFileName")
              .HasMaxLength(Constants.Strings.LONG_LENGTH);
            cpb.Property(cp => cp.Name)
              .HasColumnName("CoverPictureName")
              .HasMaxLength(Constants.Strings.LONG_LENGTH);
            cpb.Property(cp => cp.Url)
              .HasColumnName("CoverPictureUrl");
            cpb.Property(cp => cp.ThumbnailUrl)
              .HasColumnName("CoverPictureThumbnailUrl");
        });
        builder.OwnsOne(p => p.Stock, sb =>
        {
            sb.Property(s => s.Quantity)
              .HasColumnName("StockQuantity");
        });
        builder.OwnsOne(p => p.Rating, rb =>
        {
            rb.Property(r => r.Count)
              .HasColumnName("RatingCount");
            rb.Property(r => r.Value)
              .HasColumnName("RatingValue");
        });
    }

    private void ConfigureProductCategory(EntityTypeBuilder<Product> builder)
    {
        builder.OwnsMany(p => p.Categories, pcb =>
        {
            pcb.WithOwner()
               .HasForeignKey(PRODUCT_ID);

            pcb.HasOne<Category>()
               .WithMany()
               .HasForeignKey(pc => pc.CategoryId);
        });

        builder.Metadata.FindNavigation(nameof(Product.Categories))!
                        .SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    private void ConfigureProductPicture(EntityTypeBuilder<Product> builder)
    {
        builder.OwnsMany(p => p.Pictures, ppb =>
        {
            ppb.ToTable("ProductPictures");

            ppb.WithOwner();

            ppb.Property(pp => pp.FileName)
            .HasMaxLength(Constants.Strings.LONG_LENGTH);
            ppb.Property(pp => pp.Name)
            .HasMaxLength(Constants.Strings.LONG_LENGTH);
            
            ppb.HasIndex(pp => pp.FileName);
        });
        builder.Metadata.FindNavigation(nameof(Product.Pictures))!
                        .SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    public void ConfigureProductAttributes(EntityTypeBuilder<Product> builder)
    {
        builder.OwnsMany(p => p.Attributes, ab =>
        {
            ab.ToTable("Attributes");

            ab.WithOwner();

            ab.Property(a => a.Name)
              .HasMaxLength(Constants.Fields.NAME_MAXLENGTH);
        });

        builder.Metadata.FindNavigation(nameof(Product.Attributes))!
                        .SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    private void ConfigureVariations(EntityTypeBuilder<Product> builder)
    {
        builder.OwnsMany(p => p.Variations, vb =>
        {
            vb.WithOwner().HasForeignKey(PRODUCT_ID);

            vb.HasKey(ID, PRODUCT_ID);

            vb.OwnsOne(v => v.PurchasedPrice, ppb =>
            {
                ppb.Property(pp => pp.Amount)
                   .HasColumnName("PurchasedPrice")
                   .HasColumnType("money");
            });

            vb.OwnsOne(v => v.RegularPrice, rpb =>
            {
                rpb.Property(rp => rp.Amount)
                   .HasColumnName("RegularPrice")
                   .HasColumnType("money");
            });

            vb.OwnsOne(v => v.SalePrice, spb =>
            {
                spb.Property(sp => sp.Amount)
                   .HasColumnName("SalePrice")
                   .HasColumnType("money");
            });

            vb.OwnsOne(v => v.Stock, sb =>
            {
                sb.Property(s => s.Quantity)
                  .HasColumnName("StockQuantity");
            });

            vb.OwnsMany(v => v.Attributes, avb =>
            {
                avb.WithOwner()
                   .HasForeignKey(VARIATION_ID, PRODUCT_ID);

                avb.HasKey(ID, VARIATION_ID, PRODUCT_ID);

                avb.Property(av => av.Value)
                   .HasMaxLength(Constants.Strings.SMALL_LENGTH);

                avb.OwnsOne(av => av.Attribute, ab =>
                {
                    ab.Property(a => a.Name)
                      .HasColumnName("Attribute")
                      .HasMaxLength(Constants.Fields.NAME_MAXLENGTH);
                });
            });

            vb.Navigation(v => v.Attributes).Metadata
              .SetField("_attributes");

            vb.Navigation(v => v.Attributes)
              .UsePropertyAccessMode(PropertyAccessMode.Field);
        });

        builder.Metadata.FindNavigation(nameof(Product.Variations))!
                        .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
