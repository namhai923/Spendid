using MediatR;
using Microsoft.AspNetCore.Mvc;
using Spendid.Application.Expenses.Command.DeleteExpense;
using Spendid.Application.Expenses.Command.MakeExpense;
using Spendid.Application.Expenses.Command.UpdateExpense;
using Spendid.Application.Expenses.Query.GetExpense;

namespace Spendid.Api.Controllers.Expenses;

[Route("api/v{version:apiVersion}/expenses")]
public class ExpensesController(ISender sender) : ApiControllerBase
{
    private readonly ISender _sender = sender;

    [HttpGet("{expenseId}")]
    public async Task<IActionResult> GetExpense(Guid expenseId, CancellationToken cancellationToken)
    {
        var query = new GetExpenseQuery(expenseId);

        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> MakeExpense([FromBody] MakeExpenseRequest request, CancellationToken cancellationToken)
    {
        var command = new MakeExpenseCommand(
            CurrentUserId,
            request.HouseholdId,
            request.Amount,
            request.AmountCurrency,
            request.Description,
            request.Tags);

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return CreatedAtAction(nameof(GetExpense), new { expenseId = result.Value }, result.Value);
    }

    [HttpPut("{expenseId}")]
    public async Task<IActionResult> UpdateExpense(Guid expenseId, [FromBody] UpdateExpenseRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateExpenseCommand(
            CurrentUserId,
            expenseId,
            request.Amount,
            request.AmountCurrency,
            request.Description,
            request.Tags);

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return CreatedAtAction(nameof(GetExpense), new { expenseId = result.Value }, result.Value);
    }

    [HttpDelete("{expenseId}")]
    public async Task<IActionResult> DeleteExpense(Guid expenseId, CancellationToken cancellationToken)
    {
        var command = new DeleteExpenseCommand(CurrentUserId, expenseId);

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result);
    }
}
