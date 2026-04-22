namespace Spendid.Application.Abstractions.Authentication;

public interface IAuthenticationService
{
    Task<Guid> GetOrCreateUserAsync(string identityId, string email, string googleName);
}
