namespace Spendid.Application.Abstractions.Email;

public interface IEmailService
{
    Task SendAsync(IEnumerable<Domain.Users.Email> recipients, string subject, string body);
}
