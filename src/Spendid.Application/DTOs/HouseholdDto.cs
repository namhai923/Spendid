namespace Spendid.Application.DTOs;

public sealed class HouseholdDto
{
    public Guid Id { get; init; }

    public string HouseholdName { get; init; } = string.Empty;

    public UserInfoDto AdminInfo { get; init; }
}
