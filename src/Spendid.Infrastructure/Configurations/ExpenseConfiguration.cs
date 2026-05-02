using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Spendid.Domain.Expenses;
using Spendid.Domain.Households;
using Spendid.Domain.Shared;
using Spendid.Domain.Tags;
using Spendid.Domain.Users;

namespace Spendid.Infrastructure.Configurations;

internal sealed class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder.ToTable("expenses");

        builder.HasKey(expense => expense.Id);

        builder.HasOne<Household>()
           .WithMany(household =>household.Expenses)
           .HasForeignKey(expense => expense.HouseholdId)
           .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<User>()
           .WithMany()
           .HasForeignKey(expense => expense.UserId)
           .OnDelete(DeleteBehavior.Restrict);

        builder.OwnsOne(expense => expense.Amount, amountBuilder =>
        {
            amountBuilder.Property(money => money.Currency)
                .HasConversion(
                    currency => currency.Code,
                    code => Currency.FromCode(code)
                );
        });

        builder.Property(expense => expense.Description)
            .HasMaxLength(2000)
            .HasConversion(
                description => description.Value,
                value => new Description(value)
            );

        builder.HasMany<Tag>(expense => expense.Tags)
            .WithMany(tag => tag.Expenses)
            .UsingEntity(joined => joined.ToTable("expense_tags"));

        builder.Property(expense => expense.CreatedAt);
    }
}
