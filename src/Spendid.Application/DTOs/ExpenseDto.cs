namespace Spendid.Application.DTOs;

public sealed class ExpenseDto
{
    public Guid Id { get; init; }

    public UserInfoDto UserInfo { get; init; }

    public decimal Amount { get; init; }

    public string AmountCurrency { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public List<TagDto> Tags { get; init; } = [];

    public DateTime CreatedAt { get; init; }
}
