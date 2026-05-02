using Spendid.Domain.Abstractions;

namespace Spendid.Domain.Expenses;

public static class ExpenseErrors
{
    public static readonly Error NotFound = new("Expense.NotFound", "The expense with the specified identifier is not found");

    public static readonly Error Restricted = new("Expense.Restricted", "This user don't have enough permisson to perform the action");
}
