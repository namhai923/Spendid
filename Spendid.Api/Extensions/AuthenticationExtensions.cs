using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Spendid.Application.Abstractions.Authentication;
using Spendid.Domain.Constants;
using System.Security.Claims;

namespace Spendid.Api.Extensions;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddGoogleAuthentication(this IServiceCollection services, IConfiguration config)
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
                    // 2. Return the JSON error immediately
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync(new
                    {
                        error = "Unauthorized",
                        message = "This email is not allowed."
                    });

                    // 3. Tell the middleware we handled the response (don't continue)
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

        return services;
    }
}
