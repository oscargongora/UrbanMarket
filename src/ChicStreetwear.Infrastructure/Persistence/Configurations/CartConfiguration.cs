using ChicStreetwear.Domain.Aggregates.Cart;
using ChicStreetwear.Domain.ValueObjects;
using ChicStreetwear.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChicStreetwear.Infrastructure.Persistence.Configurations;
internal sealed class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    private const string ID = "Id";
    private const string CART_ID = "CartId";
    private const string CART_PRODUCT_ID = "CartProductId";
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.OwnsMany(c => c.Products, cpb =>
        {
            cpb
            .WithOwner()
            .HasForeignKey(CART_ID);

            cpb
            .HasKey(ID, CART_ID);

            cpb
            .Property(cp => cp.Name)
            .HasMaxLength(Constants.Fields.NAME_MAXLENGTH);

            cpb
            .Property(cp => cp.Price)
            .HasConversion(p => p.Amount, amount => Money.New(amount));

            cpb.OwnsMany(cv => cv.Attributes, cpab =>
            {
                cpab
                .Property<Guid>(ID);

                cpab
                .WithOwner()
                .HasForeignKey(CART_PRODUCT_ID, CART_ID);

                cpab
                .HasKey(ID, CART_PRODUCT_ID, CART_ID);

                cpab
                .Property(cva => cva.Name)
                .HasMaxLength(Constants.Fields.NAME_MAXLENGTH);

                cpab
                .Property(cva => cva.Value)
                .HasMaxLength(Constants.Strings.SMALL_LENGTH);
            });

            cpb
            .Navigation(cv => cv.Attributes).Metadata
            .SetField("_attributes");

            cpb
            .Navigation(cv => cv.Attributes)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
        });

        builder.Property(c => c.Total).HasConversion(t => t.Amount,
                                                     amount => Money.New(amount));

        builder.Metadata.FindNavigation(nameof(Cart.Products))!
                        .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
