using Asp.Versioning;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Spendid.Application.Abstractions.Authentication;
using Spendid.Application.Abstractions.Clock;
using Spendid.Application.Abstractions.Data;
using Spendid.Application.Abstractions.Email;
using Spendid.Application.DTOs;
using Spendid.Domain.Abstractions;
using Spendid.Domain.Expenses;
using Spendid.Domain.Households;
using Spendid.Domain.HouseholdUsers;
using Spendid.Domain.Tags;
using Spendid.Domain.Users;
using Spendid.Infrastructure.Authentication;
using Spendid.Infrastructure.Clock;
using Spendid.Infrastructure.Data;
using Spendid.Infrastructure.Email;
using Spendid.Infrastructure.Outbox;
using Spendid.Infrastructure.Repositories;

namespace Spendid.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();

        services.AddTransient<IEmailService, EmailService>();

        services.AddScoped<IAuthenticationService, AuthenticationService>();

        AddPersistence(services, configuration);

        AddApiVersioning(services);

        AddBackgroundJobs(services, configuration);

        return services;
    }

    private static void AddPersistence(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database") ?? throw new ArgumentNullException(nameof(configuration));

        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention());

        services.AddScoped<IHouseholdRepository, HouseholdRepository>();

        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IExpenseRepository, ExpenseRepository>();

        services.AddScoped<ITagRepository, TagRepository>();

        services.AddScoped<IHouseholdUserRepository, HouseholdUserRepository>();

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

        services.AddSingleton<ISqlConnectionFactory>(_ => new SqlConnectionFactory(connectionString));

        SqlMapper.AddTypeHandler(new JsonTypeHandler<List<TagDto>>());

        SqlMapper.AddTypeHandler(new JsonTypeHandler<UserInfoDto>());
    }

    private static void AddApiVersioning(IServiceCollection services)
    {
        services
            .AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1);
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
            .AddMvc()
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });
    }

    private static void AddBackgroundJobs(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OutboxOptions>(configuration.GetSection("Outbox"));

        services.AddQuartz();

        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

        services.ConfigureOptions<ProcessOutboxMessagesJobSetup>();
    }
}
