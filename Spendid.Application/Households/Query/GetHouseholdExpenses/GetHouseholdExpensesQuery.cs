using Spendid.Application.Abstractions.Messaging;
using Spendid.Application.DTOs;

namespace Spendid.Application.Households.Query.GetHouseholdExpenses;

public record GetHouseholdExpensesQuery(Guid HouseholdId) : IQuery<IEnumerable<ExpenseDto>>;
