using Spendid.Application.Abstractions.Messaging;
using Spendid.Domain.Abstractions;
using Spendid.Domain.Expenses;
using Spendid.Domain.Households;

namespace Spendid.Application.Households.Command.DeleteHousehold;

internal sealed class DeleteHouseholdCommandHandler(IHouseholdRepository householdRepository, IUnitOfWork unitOfWork) : ICommandHandler<DeleteHouseholdCommand>
{
    private readonly IHouseholdRepository _householdRepository = householdRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(DeleteHouseholdCommand request, CancellationToken cancellationToken)
    {
        var household = await _householdRepository.GetByIdAsync(request.HouseholdId, cancellationToken);

        if (household is null)
        {
            return Result.Failure(ExpenseErrors.NotFound);
        }

        if (household.AdminId != request.UserId)
        {
            return Result.Failure(HouseholdErrors.Restricted);
        }

        _householdRepository.Remove(household);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
