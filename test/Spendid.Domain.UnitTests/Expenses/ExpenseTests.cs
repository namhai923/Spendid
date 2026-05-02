using FluentAssertions;
using Spendid.Domain.Expenses;
using Spendid.Domain.Expenses.Events;
using Spendid.Domain.Shared;
using Spendid.Domain.UnitTests.Infrastructure;

namespace Spendid.Domain.UnitTests.Expenses;

public class ExpenseTests : BaseTest
{
    [Fact]
    public void Make_Should_SetPropertyValues()
    {
        // Act
        var expense = Expense.Make(
            ExpenseData.UserId,
            ExpenseData.HouseholdId,
            ExpenseData.Amount,
            ExpenseData.AmountCurrency,
            ExpenseData.Description,
            ExpenseData.Tags,
            ExpenseData.CreatedAt
            );

        // Assert
        expense.UserId.Should().Be( ExpenseData.UserId );
        expense.HouseholdId.Should().Be( ExpenseData.HouseholdId);
        expense.Amount.Should().Be( new Money(ExpenseData.Amount, Currency.FromCode(ExpenseData.AmountCurrency)));
        expense.Description.Should().Be( new Description(ExpenseData.Description));
        expense.Tags.Should().BeEquivalentTo( ExpenseData.Tags);
        expense.CreatedAt.Should().Be( ExpenseData.CreatedAt);
    }

    [Fact]
    public void Make_Should_RaiseExpenseMadeDomainEvent()
    {
        // Act
        var expense = Expense.Make(
            ExpenseData.UserId,
            ExpenseData.HouseholdId,
            ExpenseData.Amount,
            ExpenseData.AmountCurrency,
            ExpenseData.Description,
            ExpenseData.Tags,
            ExpenseData.CreatedAt
            );

        // Assert
        var domainEvent = AssertDomainEventWasPublished<ExpenseMadeDomainEvent>(expense);

        domainEvent.ExpenseId.Should().Be(expense.Id);
    }
}
