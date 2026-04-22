using MediatR;
using Microsoft.AspNetCore.Mvc;
using Spendid.Application.Users.Query.GetMemberships;

namespace Spendid.Api.Controllers.Users;

[Route("api/v{version:apiVersion}/current-user")]
public class UsersController(ISender sender) : ApiControllerBase
{
    private readonly ISender _sender = sender;

    [HttpGet]
    public IActionResult GetCurrentUser()
    {
        return Ok(new {
            UserName = CurrentUserName,
            Email = CurrentUserEmail,
            IsAuthenticated = true });
    }

    [HttpGet("memberships")]
    public async Task<IActionResult> GetMemberships(CancellationToken cancellationToken)
    {
        var query = new GetMembershipsQuery(CurrentUserId);

        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound();
    }
}
