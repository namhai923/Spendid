using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spendid.Domain.Constants;
using System.Security.Claims;

namespace Spendid.Api.Controllers;

[Authorize]
[ApiController]
[ApiVersion(ApiVersions.V1)]
public abstract class ApiControllerBase : ControllerBase
{
    protected Guid CurrentUserId
    {
        get
        {
            var idClaim = User.FindFirstValue(CustomClaimTypes.DbId);
            return Guid.TryParse(idClaim, out var guid) ? guid : Guid.Empty;
        }
    }

    protected string CurrentUserName => User.Identity?.Name ?? "Unknown User";

    protected string? CurrentUserEmail => User.FindFirstValue(ClaimTypes.Email);
}
