using Spendid.Application.Abstractions.Email;

namespace Spendid.Infrastructure.Email;

internal sealed class EmailService : IEmailService
{
    public Task SendAsync(IEnumerable<Domain.Users.Email> recipients, string subject, string body)
    {
        return Task.CompletedTask;
    }
}
