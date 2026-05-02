using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Spendid.Infrastructure;

namespace Spendid.Application.IntegrationTests.Infrastructure;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
{
    private readonly IServiceScope _scope;
    protected readonly ApplicationDbContext DbContext;
    protected readonly ISender Sender;
    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        _scope = factory.Services.CreateScope();

        DbContext = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        Sender = _scope.ServiceProvider.GetRequiredService<ISender>();
    }
}
