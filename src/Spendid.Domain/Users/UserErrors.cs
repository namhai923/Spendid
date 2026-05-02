using Spendid.Domain.Abstractions;

namespace Spendid.Domain.Users;

public static class UserErrors
{
    public static readonly Error NotFound = new("User.NotFound", "The user with the specified identifier is not found");
}
