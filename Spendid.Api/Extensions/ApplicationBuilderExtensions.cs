using Microsoft.EntityFrameworkCore;
using Npgsql;
using Polly;
using Spendid.Api.Middlewares;
using Spendid.Infrastructure;

namespace Spendid.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        Policy
          .Handle<NpgsqlException>()
          .WaitAndRetry(5, _ => TimeSpan.FromSeconds(2))
          .Execute(() => dbContext.Database.Migrate());
    }

    public static void UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
