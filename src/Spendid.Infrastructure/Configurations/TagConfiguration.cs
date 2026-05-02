using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Spendid.Domain.Households;
using Spendid.Domain.Tags;

namespace Spendid.Infrastructure.Configurations;

internal sealed class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.ToTable("tags");

        builder.HasKey(tag => tag.Id);

        builder.HasOne<Household>()
            .WithMany(household => household.Tags)
            .HasForeignKey(tag => tag.HouseholdId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(tag => tag.TagName)
            .HasMaxLength(200)
            .HasConversion(
                tagName => tagName.Value,
                value => new TagName(value)
            );

        builder.Property(tag => tag.Color)
            .HasMaxLength(7)
            .IsRequired()
            .HasColumnType("varchar(7)");
    }
}
