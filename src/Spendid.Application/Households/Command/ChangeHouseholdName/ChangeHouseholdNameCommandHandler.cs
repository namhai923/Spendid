using Spendid.Application.Abstractions.Messaging;
using Spendid.Domain.Abstractions;
using Spendid.Domain.Households;

namespace Spendid.Application.Households.Command.ChangeHouseholdName;

internal sealed class ChangeHouseholdNameCommandHandler(IHouseholdRepository householdRepository, IUnitOfWork unitOfWork) : ICommandHandler<ChangeHouseholdNameCommand>
{
    private readonly IHouseholdRepository _householdRepository = householdRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(ChangeHouseholdNameCommand request, CancellationToken cancellationToken)
    {
        Household? household = await _householdRepository.GetByIdAsync(request.HouseholdId, cancellationToken);

        if (household is null)
        {
            return Result.Failure(HouseholdErrors.NotFound);
        }

        if (request.AdminId != household.AdminId)
        {
            return Result.Failure(HouseholdErrors.Restricted);
        }

        household.ChangeName(request.HouseholdName);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
