namespace Spendid.Application.DTOs;

public sealed class TagDto
{
    public Guid Id { get; init; }

    public string TagName { get; init; } = string.Empty;

    public string Color { get; init; } = string.Empty;
}
