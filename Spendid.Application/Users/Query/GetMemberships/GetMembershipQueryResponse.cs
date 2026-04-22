using Spendid.Domain.HouseholdUsers;

namespace Spendid.Application.Users.Query.GetMemberships;

public sealed class GetMembershipQueryResponse
{
    public Guid HouseholdId { get; init; }

    public string HouseholdName { get; init; } = string.Empty;

    public UserRole Role { get; init; }
}
