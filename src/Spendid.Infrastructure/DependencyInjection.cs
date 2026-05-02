using Asp.Versioning;
using Dapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Spendid.Application.Abstractions.Authentication;
using Spendid.Application.Abstractions.Caching;
using Spendid.Application.Abstractions.Clock;
using Spendid.Application.Abstractions.Data;
using Spendid.Application.Abstractions.Email;
using Spendid.Application.DTOs;
using Spendid.Domain.Abstractions;
using Spendid.Domain.Constants;
using Spendid.Domain.Expenses;
using Spendid.Domain.Households;
using Spendid.Domain.HouseholdUsers;
using Spendid.Domain.Tags;
using Spendid.Domain.Users;
using Spendid.Infrastructure.Authentication;
using Spendid.Infrastructure.Caching;
using Spendid.Infrastructure.Clock;
using Spendid.Infrastructure.Data;
using Spendid.Infrastructure.Email;
using Spendid.Infrastructure.Outbox;
using Spendid.Infrastructure.Repositories;
using System.Security.Claims;

namespace Spendid.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();

        services.AddTransient<IEmailService, EmailService>();

        services.AddScoped<IAuthenticationService, AuthenticationService>();

        AddPersistence(services, configuration);

        AddBackgroundJobs(services, configuration);

        AddGoogleAuthentication(services, configuration);

        AddCaching(services, configuration);

        AddHealthChecks(services, configuration);

        AddApiVersioning(services);

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

    public static void AddGoogleAuthentication(IServiceCollection services, IConfiguration config)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
        })
        .AddCookie()
        //.AddJwtBearer(options =>
        //{
        //    options.Authority = "https://accounts.google.com";
        //    options.TokenValidationParameters = new TokenValidationParameters
        //    {
        //        ValidateAudience = true,
        //        ValidAudience = config["Authentication:Google:ClientId"],
        //        ValidateIssuer = true,
        //        ValidIssuer = "https://accounts.google.com"
        //    };
        //})
        .AddGoogle(options =>
        {
            options.ClientId = config["Authentication:Google:ClientId"]!;
            options.ClientSecret = config["Authentication:Google:ClientSecret"]!;

            options.Events.OnTicketReceived = async context =>
            {
                var identityId = context.Principal?.FindFirstValue(ClaimTypes.NameIdentifier);
                var email = context.Principal?.FindFirstValue(ClaimTypes.Email);
                var googleName = context.Principal?.FindFirstValue(ClaimTypes.Name) ?? "Unknown User";


                var allowedEmails = (config["Authorization:AllowedEmails"] ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries)
                                 .Select(e => e.Trim())
                                 .ToList();

                if (string.IsNullOrEmpty(email) || !allowedEmails.Contains(email))
                {
                    context.Fail("Unauthorized email.");

                    context.Response.StatusCode = StatusCodes.Status403Forbidden;

                    context.Response.ContentType = "application/json";

                    await context.Response.WriteAsJsonAsync(new
                    {
                        error = "Unauthorized",
                        message = "This email is not allowed."
                    });

                    context.HandleResponse();

                    return;
                }

                var authenticationService = context.HttpContext.RequestServices.GetRequiredService<IAuthenticationService>();

                Guid internalUserId = await authenticationService.GetOrCreateUserAsync(identityId!, email, googleName);

                var claimsIdentity = (ClaimsIdentity)context.Principal!.Identity!;
                claimsIdentity.AddClaim(new Claim(CustomClaimTypes.DbId, internalUserId.ToString()));
            };
        });
        //.AddPolicyScheme("SmartAuth", "JWT or Cookie", options =>
        //{
        //    options.ForwardDefaultSelector = context =>
        //    {
        //        var authHeader = context.Request.Headers.Authorization.ToString();
        //        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
        //        {
        //            return JwtBearerDefaults.AuthenticationScheme;
        //        }
        //        return CookieAuthenticationDefaults.AuthenticationScheme;
        //    };
        //});
    }

    public static void AddCaching(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Cache") ?? throw new ArgumentNullException(nameof(configuration));

        services.AddStackExchangeRedisCache(options => options.Configuration = connectionString);

        services.AddSingleton<ICacheService, CacheService>();
    }

    public static void AddHealthChecks(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks().AddNpgSql(configuration.GetConnectionString("Database")!).AddRedis(configuration.GetConnectionString("Cache")!);
    }
}
