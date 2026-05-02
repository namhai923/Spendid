using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using Spendid.Api.Extensions;
using Spendid.Application;
using Spendid.Infrastructure;
using Serilog;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddControllers();

builder.Services.AddOpenApi();

//builder.Services.AddOpenApi(options =>
//{
//    options.AddDocumentTransformer((document, context, cancellationToken) =>
//    {
//        var scheme = new OpenApiSecurityScheme
//        {
//            Type = SecuritySchemeType.OAuth2,
//            //Scheme = "Bearer",
//            BearerFormat = "JWT",
//            In = ParameterLocation.Header,
//            Flows = new OpenApiOAuthFlows
//            {
//                AuthorizationCode = new OpenApiOAuthFlow
//                {
//                    AuthorizationUrl = new Uri(GoogleDefaults.AuthorizationEndpoint),
//                    TokenUrl = new Uri(GoogleDefaults.TokenEndpoint),
//                    Scopes = new Dictionary<string, string> {
//                        { "openid", "OpenID" },
//                        { "profile", "User profile" },
//                        { "email", "User email" }
//                    }
//                }
//            }
//        };
//        document.Components ??= new OpenApiComponents();
//        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
//        document.Components.SecuritySchemes.Add(GoogleDefaults.AuthenticationScheme, scheme);

//        document.Security ??= [];
//        document.Security.Add(new OpenApiSecurityRequirement
//        {
//            [new OpenApiSecuritySchemeReference(GoogleDefaults.AuthenticationScheme, document)] = ["openid", "profile", "email"]
//        });

//        return Task.CompletedTask;
//    });
//});

builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("Spendid API")
            .WithTheme(ScalarTheme.BluePlanet)
            .WithDefaultHttpClient(ScalarTarget.Http, ScalarClient.Http11);
            //.AddAuthorizationCodeFlow(GoogleDefaults.AuthenticationScheme, flow =>
            // {
            //     flow.WithClientId(builder.Configuration["Authentication:Google:ClientId"])
            //         .WithClientSecret(builder.Configuration["Authentication:Google:ClientSecret"])
            //         .WithPkce(Pkce.Sha256);
            // });
    });

    app.ApplyMigrations();

    app.SeedData();
}

app.UseHttpsRedirection();

app.UseRequestContextLogging();

app.UseSerilogRequestLogging();

app.UseCustomExceptionHandler();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();

public partial class Program;
