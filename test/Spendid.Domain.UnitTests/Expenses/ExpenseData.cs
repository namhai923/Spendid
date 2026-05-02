using Spendid.Domain.Tags;

namespace Spendid.Domain.UnitTests.Expenses;

internal static class ExpenseData
{
    public static readonly Guid UserId = Guid.NewGuid();
    public static readonly Guid HouseholdId = Guid.NewGuid();
    public static readonly decimal Amount= 0;
    public static readonly string AmountCurrency = "USD";
    public static readonly string Description = "";
    public static readonly List<Tag> Tags = [];
    public static readonly DateTime CreatedAt = DateTime.UtcNow;
}
