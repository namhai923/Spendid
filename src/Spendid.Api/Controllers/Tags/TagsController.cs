using MediatR;
using Microsoft.AspNetCore.Mvc;
using Spendid.Application.Expenses.Command.DeleteExpense;
using Spendid.Application.Tags.Command.CreateTag;
using Spendid.Application.Tags.Command.DeleteTag;
using Spendid.Application.Tags.Command.UpdateTag;
using Spendid.Domain.Abstractions;

namespace Spendid.Api.Controllers.Tags;

[Route("api/v{version:apiVersion}/tags")]
public class TagsController(ISender sender) : ApiControllerBase
{
    private readonly ISender _sender = sender;

    [HttpPost]
    public async Task<IActionResult> CreateTag([FromBody] CreateTagRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateTagCommand(CurrentUserId, request.HouseholdId, request.TagName, request.Color);

        Result result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result);
    }

    [HttpPut("{tagId}")]
    public async Task<IActionResult> UpdateTag(Guid tagId, [FromBody] UpdateTagRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateTagCommand(CurrentUserId, tagId, request.TagName, request.Color);

        Result result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result);
    }

    [HttpDelete("{tagId}")]
    public async Task<IActionResult> DeleteTag(Guid tagId, CancellationToken cancellationToken)
    {
        var command = new DeleteTagCommand(CurrentUserId, tagId);

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result);
    }
}
