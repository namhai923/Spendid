using Spendid.Domain.Expenses;
using Spendid.Domain.Households;
using Spendid.Domain.HouseholdUsers;
using Spendid.Domain.Tags;

namespace Spendid.Domain.UnitTests.Households;

internal static class HouseholdData
{
    public static readonly string HouseholdName = "HouseholdName";
    public static readonly Guid AdminId = Guid.NewGuid();
    public static readonly List<Tag> Tags = [];
    public static readonly List<Expense> Expenses = [];
    public static readonly List<HouseholdUser> Members = [];
}
