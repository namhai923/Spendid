using Spendid.Domain.HouseholdUsers;

namespace Spendid.Application.Households.Query.GetHouseholdUsers;

public sealed class GetHouseholdUsersQueryResponse
{
    public string Email { get; init; } = string.Empty;

    public string UserName { get; init; } = string.Empty;

    public UserRole Role { get; init; }
}
