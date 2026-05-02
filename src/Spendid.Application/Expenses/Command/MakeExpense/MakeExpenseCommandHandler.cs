using Spendid.Application.Abstractions.Clock;
using Spendid.Application.Abstractions.Messaging;
using Spendid.Domain.Abstractions;
using Spendid.Domain.Expenses;
using Spendid.Domain.Households;
using Spendid.Domain.Tags;

namespace Spendid.Application.Expenses.Command.MakeExpense;

internal sealed class MakeExpenseCommandHandler(IHouseholdRepository householdRepository, IExpenseRepository expenseRepository, ITagRepository tagRepository, IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider) : ICommandHandler<MakeExpenseCommand, Guid>
{
    private readonly IHouseholdRepository _householdRepository = householdRepository;
    private readonly IExpenseRepository _expenseRepository = expenseRepository;
    private readonly ITagRepository _tagRepository = tagRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;

    public async Task<Result<Guid>> Handle(MakeExpenseCommand request, CancellationToken cancellationToken)
    {
        var household = await _householdRepository.GetByIdWithMembersAsync(request.HouseholdId, cancellationToken);

        if (household is null) {
            return Result.Failure<Guid>(HouseholdErrors.NotFound);
        }

        if (!household.Members.Any(m => m.UserId == request.UserId))
        {
            return Result.Failure<Guid>(HouseholdErrors.MustBeMember);
        }

        var tags = await _tagRepository.GetByIdsAsync(request.Tags, request.HouseholdId, cancellationToken);

        var requestedCount = request.Tags.Distinct().Count();

        if (tags.Count != requestedCount)
        {
            return Result.Failure<Guid>(TagErrors.NotFound);
        }

        var expense = Expense.Make(request.UserId, household.Id, request.Amount, request.AmountCurrency, request.Description, tags, _dateTimeProvider.UtcNow);

        _expenseRepository.Add(expense);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return expense.Id;
    }
}
