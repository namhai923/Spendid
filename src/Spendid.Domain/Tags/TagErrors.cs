using Spendid.Domain.Abstractions;

namespace Spendid.Domain.Tags;

public static class TagErrors
{
    public static readonly Error NotFound = new("Tag.NotFound", "The tag with the specified identifier is not found");
}
