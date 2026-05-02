using Spendid.Application.Abstractions.Messaging;
using Spendid.Domain.Abstractions;
using Spendid.Domain.Households;
using Spendid.Domain.HouseholdUsers;

namespace Spendid.Application.Households.Command.CreateHousehold;

internal sealed class CreateHouseholdCommandHandler(IHouseholdRepository householdRepository, IHouseholdUserRepository householdUserRepository, IUnitOfWork unitOfWork) : ICommandHandler<CreateHouseholdCommand>
{
    private readonly IHouseholdRepository _householdRepository = householdRepository;
    private readonly IHouseholdUserRepository _householdUserRepository = householdUserRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(CreateHouseholdCommand request, CancellationToken cancellationToken)
    {
        var household = Household.Create(request.HouseholdName, request.UserId);

        _householdRepository.Add(household);

        var householdUser = new HouseholdUser(household.Id, request.UserId, UserRole.Admin);

        _householdUserRepository.Add(householdUser);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
