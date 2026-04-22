namespace Spendid.Api.Controllers.Expenses;

public record UpdateExpenseRequest(
    decimal Amount,
    string AmountCurrency,
    string Description,
    List<Guid> Tags);
