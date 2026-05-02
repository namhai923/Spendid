using Spendid.Domain.Abstractions;

namespace Spendid.Domain.Households;

public static class HouseholdErrors
{
    public static readonly Error NotFound = new("Household.NotFound", "The household with the specified identifier is not found");

    public static readonly Error Restricted = new("Household.Restricted", "This user don't have enough permisson to perform the action");

    public static readonly Error MustBeMember = new("Household.MustBeMember", "User must be a member in this household");

    public static readonly Error AlreadyMember = new("Household.AlreadyMember", "User is already a member in this household");
}
