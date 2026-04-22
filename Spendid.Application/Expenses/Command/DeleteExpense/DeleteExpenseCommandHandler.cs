using Spendid.Application.Abstractions.Messaging;
using Spendid.Domain.Abstractions;
using Spendid.Domain.Expenses;
using Spendid.Domain.Households;
using Spendid.Domain.Users;

namespace Spendid.Application.Expenses.Command.DeleteExpense;

internal sealed class DeleteExpenseCommandHandler(IExpenseRepository expenseRepository, IHouseholdRepository householdRepository, IUnitOfWork unitOfWork) : ICommandHandler<DeleteExpenseCommand>
{
    private readonly IExpenseRepository _expenseRepository = expenseRepository;
    private readonly IHouseholdRepository _householdRepository = householdRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = await _expenseRepository.GetByIdAsync(request.ExpenseId, cancellationToken);

        if (expense is null)
        {
            return Result.Failure(ExpenseErrors.NotFound);
        }

        var household = await _householdRepository.GetByIdWithMembersAsync(expense.HouseholdId, cancellationToken);

        if (household is null)
        {
            return Result.Failure(HouseholdErrors.NotFound);
        }

        var member = household.Members.FirstOrDefault(m => m.UserId == request.UserId);

        var isOwner = expense.UserId == request.UserId;

        var isAdmin = request.UserId == household.AdminId;

        if (member is null || (!isOwner && !isAdmin))
        {
            return Result.Failure(ExpenseErrors.Restricted);
        }

        _expenseRepository.Remove(expense);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
