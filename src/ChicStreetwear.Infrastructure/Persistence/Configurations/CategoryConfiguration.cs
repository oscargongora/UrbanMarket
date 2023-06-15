using ChicStreetwear.Domain.Aggregates.Category;
using ChicStreetwear.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChicStreetwear.Infrastructure.Persistence.Configurations;
internal sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.Property(c => c.Name)
               .HasMaxLength(Constants.Fields.NAME_MAXLENGTH)
               .IsRequired();

        builder.Property(c => c.Description)
               .HasMaxLength(Constants.Fields.DESCRIPTION_MAXLENGTH)
               .IsRequired();
    }
}
