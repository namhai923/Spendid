namespace Spendid.Api.Controllers.Expenses;

public sealed record MakeExpenseRequest(
    Guid HouseholdId,
    decimal Amount,
    string AmountCurrency,
    string Description,
    List<Guid> Tags);
