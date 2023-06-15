using ChicStreetwear.Domain.Aggregates.User;
using ChicStreetwear.Domain.ValueObjects;
using ChicStreetwear.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChicStreetwear.Infrastructure.Persistence.Configurations;
internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.UserName)
            .HasMaxLength(Constants.Fields.USERNAME_MAXLENGTH);

        builder.HasIndex(u => u.ApplicationUserId);

        builder.OwnsMany(u => u.Addresses, ab =>
        {
            ab.Property(a => a.FirstName)
             .HasMaxLength(Constants.Fields.NAME_MAXLENGTH)
             .HasColumnName(nameof(Address.FirstName));

            ab.Property(a => a.LastName)
               .HasMaxLength(Constants.Fields.NAME_MAXLENGTH)
               .HasColumnName(nameof(Address.LastName));

            ab.Property(a => a.Email)
               .HasMaxLength(Constants.Strings.LONG_LENGTH)
               .HasColumnName(nameof(Address.Email));

            ab.Property(a => a.PhoneNumber)
               .HasMaxLength(Constants.Strings.SMALL_LENGTH)
               .HasColumnName(nameof(Address.PhoneNumber));

            ab.Property(a => a.AddressLine1)
               .HasMaxLength(Constants.Strings.LONG_LENGTH)
               .HasColumnName(nameof(Address.AddressLine1));

            ab.Property(a => a.AddressLine2)
               .HasMaxLength(Constants.Strings.LONG_LENGTH)
               .HasColumnName(nameof(Address.AddressLine2));

            ab.Property(a => a.Country)
               .HasMaxLength(Constants.Strings.MEDIUM_LENGTH)
               .HasColumnName(nameof(Address.Country));

            ab.Property(a => a.City)
               .HasMaxLength(Constants.Strings.MEDIUM_LENGTH)
               .HasColumnName(nameof(Address.City));

            ab.Property(a => a.State)
               .HasMaxLength(Constants.Strings.MEDIUM_LENGTH)
               .HasColumnName(nameof(Address.State));

            ab.Property(a => a.PostalCode)
               .HasMaxLength(Constants.Strings.SMALL_LENGTH)
               .HasColumnName(nameof(Address.PostalCode));
        });
    }
}
