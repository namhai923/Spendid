using MediatR;
using Microsoft.AspNetCore.Mvc;
using Spendid.Application.Households.Command.AddHouseholdMember;
using Spendid.Application.Households.Command.ChangeHouseholdAdmin;
using Spendid.Application.Households.Command.ChangeHouseholdName;
using Spendid.Application.Households.Command.CreateHousehold;
using Spendid.Application.Households.Command.DeleteHousehold;
using Spendid.Application.Households.Command.RemoveHouseholdMember;
using Spendid.Application.Households.Query.GetHousehold;
using Spendid.Application.Households.Query.GetHouseholdExpenses;
using Spendid.Application.Households.Query.GetHouseholdTags;
using Spendid.Application.Households.Query.GetHouseholdUsers;
using Spendid.Domain.Abstractions;

namespace Spendid.Api.Controllers.Households;

[Route("api/v{version:apiVersion}/households")]
public class HouseholdsController(ISender sender) : ApiControllerBase
{
    private readonly ISender _sender = sender;

    [HttpGet("{householdId}")]
    public async Task<IActionResult> GetHousehold(Guid householdId, CancellationToken cancellationToken)
    {
        var query = new GetHouseholdQuery(householdId);

        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound();
    }

    [HttpGet("{householdId}/expenses")]
    public async Task<IActionResult> GetExpenses(Guid householdId, CancellationToken cancellationToken)
    {
        var query = new GetHouseholdExpensesQuery(householdId);

        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound();
    }

    [HttpGet("{householdId}/tags")]
    public async Task<IActionResult> GetTags(Guid householdId, CancellationToken cancellationToken)
    {
        var query = new GetHouseholdTagsQuery(householdId);

        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound();
    }

    [HttpGet("{householdId}/users")]
    public async Task<IActionResult> GetMembers(Guid householdId, CancellationToken cancellationToken)
    {
        var query = new GetHouseholdUsersQuery(householdId);

        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateHouseholdRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateHouseholdCommand(CurrentUserId, request.HouseholdName);

        Result result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result);
    }

    [HttpPost("{householdId}/members")]
    public async Task<IActionResult> AddMember(Guid householdId, [FromBody] AddMemberRequest request, CancellationToken cancellationToken)
    {
        var command = new AddHouseholdMemberCommand(CurrentUserId, householdId, request.UserEmail);

        Result result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result);
    }

    [HttpPut("{householdId}/name")]
    public async Task<IActionResult> ChangeName(Guid householdId, [FromBody] ChangeNameRequest request, CancellationToken cancellationToken)
    {
        var command = new ChangeHouseholdNameCommand(CurrentUserId, householdId, request.NewName);

        Result result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result);
    }

    [HttpPut("{householdId}/admin")]
    public async Task<IActionResult> ChangeAdmin(Guid householdId, [FromBody] ChangeAdminRequest request, CancellationToken cancellationToken)
    {
        var command = new ChangeHouseholdAdminCommand(CurrentUserId, householdId, request.NewAdminEmail);

        Result result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result);
    }

    [HttpDelete("{householdId}/members/{userEmail}")]
    public async Task<IActionResult> RemoveMember(Guid householdId, string userEmail, CancellationToken cancellationToken)
    {
        var command = new RemoveHouseholdMemberCommand(CurrentUserId, householdId, userEmail);

        Result result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result);
    }

    [HttpDelete("{householdId}")]
    public async Task<IActionResult> DeleteHousehold(Guid householdId, CancellationToken cancellationToken)
    {
        var command = new DeleteHouseholdCommand(CurrentUserId, householdId);

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result);
    }
}
