using ChicStreetwear.Domain.Aggregates.Order;
using ChicStreetwear.Domain.ValueObjects;
using ChicStreetwear.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChicStreetwear.Infrastructure.Persistence.Configurations;
internal sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    private const string ID = "Id";
    private const string ORDER_ID = "OrderId";
    private const string ORDER_PRODUCT_ID = "OrderProductId";
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.OwnsMany(c => c.Products, opb =>
        {
            opb
            .WithOwner()
            .HasForeignKey(ORDER_ID);

            opb
            .HasKey(ID, ORDER_ID);

            opb
            .Property(op => op.Name)
            .HasMaxLength(Constants.Fields.NAME_MAXLENGTH);

            opb
            .Property(op => op.Price)
            .HasConversion(p => p.Amount, amount => Money.New(amount));

            opb.OwnsMany(cv => cv.Attributes, opab =>
            {
                opab
                .Property<Guid>(ID);

                opab
                .WithOwner()
                .HasForeignKey(ORDER_PRODUCT_ID, ORDER_ID);

                opab
                .HasKey(ID, ORDER_PRODUCT_ID, ORDER_ID);

                opab
                .Property(cva => cva.Name)
                .HasMaxLength(Constants.Fields.NAME_MAXLENGTH);

                opab
                .Property(cva => cva.Value)
                .HasMaxLength(Constants.Strings.SMALL_LENGTH);
            });

            opb
            .Navigation(cv => cv.Attributes).Metadata
            .SetField("_attributes");

            opb
            .Navigation(cv => cv.Attributes)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
        });

        builder.OwnsOne(o => o.DeliveredAddress, dab =>
        {
            dab.Property(da => da.FullName)
               .HasMaxLength(Constants.Fields.NAME_MAXLENGTH)
               .HasColumnName(nameof(Address.FullName));

            dab.Property(da => da.FirstName)
               .HasMaxLength(Constants.Fields.NAME_MAXLENGTH)
               .HasColumnName(nameof(Address.FirstName));

            dab.Property(da => da.LastName)
               .HasMaxLength(Constants.Fields.NAME_MAXLENGTH)
               .HasColumnName(nameof(Address.LastName));

            dab.Property(da => da.Email)
               .HasMaxLength(Constants.Strings.LONG_LENGTH)
               .HasColumnName(nameof(Address.Email));

            dab.Property(da => da.PhoneNumber)
               .HasMaxLength(Constants.Strings.SMALL_LENGTH)
               .HasColumnName(nameof(Address.PhoneNumber));

            dab.Property(da => da.AddressLine1)
               .HasMaxLength(Constants.Strings.LONG_LENGTH)
               .HasColumnName(nameof(Address.AddressLine1));

            dab.Property(da => da.AddressLine2)
               .HasMaxLength(Constants.Strings.LONG_LENGTH)
               .HasColumnName(nameof(Address.AddressLine2));

            dab.Property(da => da.Country)
               .HasMaxLength(Constants.Strings.MEDIUM_LENGTH)
               .HasColumnName(nameof(Address.Country));

            dab.Property(da => da.City)
               .HasMaxLength(Constants.Strings.MEDIUM_LENGTH)
               .HasColumnName(nameof(Address.City));

            dab.Property(da => da.State)
               .HasMaxLength(Constants.Strings.MEDIUM_LENGTH)
               .HasColumnName(nameof(Address.State));

            dab.Property(da => da.PostalCode)
               .HasMaxLength(Constants.Strings.SMALL_LENGTH)
               .HasColumnName(nameof(Address.PostalCode));
        });

        builder.Property(c => c.Total).HasConversion(t => t.Amount,
                                                     amount => Money.New(amount));

        builder.Property(o => o.ReceiptEmail)
            .HasMaxLength(Constants.Strings.LONG_LENGTH);

        builder.Metadata.FindNavigation(nameof(Order.Products))!
                        .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
