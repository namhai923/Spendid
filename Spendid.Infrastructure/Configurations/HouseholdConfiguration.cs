using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Spendid.Domain.Households;
using Spendid.Domain.Users;

namespace Spendid.Infrastructure.Configurations;

internal sealed class HouseholdConfiguration : IEntityTypeConfiguration<Household>
{
    public void Configure(EntityTypeBuilder<Household> builder)
    {
        builder.ToTable("households");

        builder.HasKey(household => household.Id);

        builder.Property(household => household.HouseholdName)
            .HasMaxLength(200)
            .HasConversion(householdName => householdName.Value, value => new HouseholdName(value));

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(household => household.AdminId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(household => household.Tags)
            .WithOne()
            .HasForeignKey(tag => tag.HouseholdId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(household => household.Expenses)
            .WithOne()
            .HasForeignKey(expense => expense.HouseholdId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property<uint>("Version").IsRowVersion();
    }
}
