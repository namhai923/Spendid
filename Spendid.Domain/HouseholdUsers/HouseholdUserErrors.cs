using Spendid.Domain.Abstractions;

namespace Spendid.Domain.HouseholdUsers;

public static class HouseholdUserErrors
{
    public static readonly Error Overlapped = new("HouseholdUser.Overlapped", "The user is already in the household");
}
