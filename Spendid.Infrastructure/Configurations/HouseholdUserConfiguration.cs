using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Spendid.Domain.HouseholdUsers;

namespace Spendid.Infrastructure.Configurations;

internal sealed class HouseholdUserConfiguration : IEntityTypeConfiguration<HouseholdUser>
{
    public void Configure(EntityTypeBuilder<HouseholdUser> builder)
    {
        builder.ToTable("household_users");

        builder.HasKey(householdUser => householdUser.Id);

        builder.HasIndex(householdUser => new { householdUser.HouseholdId, householdUser.UserId })
            .IsUnique();

        builder.HasOne(householdUser => householdUser.Household)
            .WithMany(household => household.Members)
            .HasForeignKey(householdUser => householdUser.HouseholdId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(householdUser => householdUser.User)
            .WithMany(user => user.HouseholdMemberships)
            .HasForeignKey(householdUser => householdUser.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(householdUser => householdUser.Role)
            .IsRequired()
            .HasDefaultValue(UserRole.Member)
            .HasSentinel((UserRole)(-1));
    }
}
