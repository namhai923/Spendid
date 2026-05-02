using Spendid.Application.Abstractions.Messaging;
using Spendid.Domain.Abstractions;
using Spendid.Domain.Expenses;
using Spendid.Domain.Tags;

namespace Spendid.Application.Expenses.Command.UpdateExpense;

internal sealed class UpdateExpenseCommandHandler(IExpenseRepository expenseRepository, ITagRepository tagRepository, IUnitOfWork unitOfWork) : ICommandHandler<UpdateExpenseCommand, Guid>
{
    private readonly IExpenseRepository _expenseRepository = expenseRepository;
    private readonly ITagRepository _tagRepository = tagRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = await _expenseRepository.GetByIdAsync(request.ExpenseId, cancellationToken);

        if (expense is null)
        {
            return Result.Failure<Guid>(ExpenseErrors.NotFound);
        }

        if (expense.UserId != request.UserId)
        {
            return Result.Failure<Guid>(ExpenseErrors.Restricted);
        }

        var tags = await _tagRepository.GetByIdsAsync(request.Tags, expense.HouseholdId, cancellationToken);

        if (tags.Count != request.Tags.Distinct().Count())
        {
            return Result.Failure<Guid>(TagErrors.NotFound);
        }

        expense.Update(request.Amount, request.AmountCurrency, request.Description, tags);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return expense.Id;
    }
}
