using Spendid.Domain.Households;

namespace Spendid.Application.UnitTests.Households;

internal static class HouseholdData
{
    public static Household Create() => Household.Create(HouseholdName, AdminId);

    private static readonly string HouseholdName = "HouseholdName";
    private static readonly Guid AdminId = Guid.NewGuid();
}
 