using ChicStreetwear.Domain.Aggregates.ProductReview;
using ChicStreetwear.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChicStreetwear.Infrastructure.Persistence.Configurations;
internal class ProductReviewConfiguration : IEntityTypeConfiguration<ProductReview>
{
    public void Configure(EntityTypeBuilder<ProductReview> builder)
    {
        builder.Property(pr => pr.Comment)
               .HasMaxLength(Constants.Strings.EXTRA_LONG_LENGTH);
    }
}
