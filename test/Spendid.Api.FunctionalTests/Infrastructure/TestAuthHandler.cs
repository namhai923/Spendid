using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Spendid.Api.FunctionalTests.Infrastructure;

public class TestAuthHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    IServiceProvider serviceProvider) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var identityId = "google-test-id-123";
        var email = "test@example.com";
        var name = "Test User";

        using var scope = _serviceProvider.CreateScope();
        var authService = scope.ServiceProvider.GetRequiredService<Application.Abstractions.Authentication.IAuthenticationService>();

        Guid internalUserId = await authService.GetOrCreateUserAsync(identityId, email, name);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, identityId),
            new(ClaimTypes.Email, email),
            new(ClaimTypes.Name, name),
            new("DbId", internalUserId.ToString())
        };

        var identity = new ClaimsIdentity(claims, "TestScheme");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "TestScheme");

        return AuthenticateResult.Success(ticket);
    }
}