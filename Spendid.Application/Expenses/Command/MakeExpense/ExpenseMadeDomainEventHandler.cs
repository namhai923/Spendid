using MediatR;
using Spendid.Application.Abstractions.Email;
using Spendid.Domain.Expenses;
using Spendid.Domain.Expenses.Events;
using Spendid.Domain.Households;
using Spendid.Domain.Users;

namespace Spendid.Application.Expenses.Command.MakeExpense;

internal sealed class ExpenseMadeDomainEventHandler(IExpenseRepository expenseRepository, IHouseholdRepository householdRepository, IUserRepository userRepository, IEmailService emailService) : INotificationHandler<ExpenseMadeDomainEvent>
{
    private readonly IExpenseRepository _expenseRepository = expenseRepository;

    private readonly IHouseholdRepository _householdRepository = householdRepository;

    private readonly IUserRepository _userRepository = userRepository;

    private readonly IEmailService _emailService = emailService;
    public async Task Handle(ExpenseMadeDomainEvent notification, CancellationToken cancellationToken)
    {
        var expense = await _expenseRepository.GetByIdAsync(notification.ExpenseId, cancellationToken);

        if (expense is null)
        {
            return;
        }

        var household = await _householdRepository.GetByIdAsync(expense.HouseholdId, cancellationToken);

        if (household is null)
        {
            return;
        }

        var members = await _userRepository.GetByIdsAsync([.. household.Members.Select(member => member.Id)], cancellationToken);

        if (members is null)
        {
            return;
        }

        await _emailService.SendAsync([.. members.Select(x => x.Email)], "New expense just made", $"Value: {expense.Amount}\n Description: {expense.Description}");
    }
}
