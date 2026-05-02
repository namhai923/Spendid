using Spendid.Application.Abstractions.Messaging;
using Spendid.Domain.Abstractions;
using Spendid.Domain.Households;
using Spendid.Domain.HouseholdUsers;
using Spendid.Domain.Users;

namespace Spendid.Application.Households.Command.AddHouseholdMember;

internal sealed class AddHouseholdMemberCommandHandler(IHouseholdRepository householdRepository, IUserRepository userRepository, IUnitOfWork unitOfWork) : ICommandHandler<AddHouseholdMemberCommand>
{
    private readonly IHouseholdRepository _householdRepository = householdRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(AddHouseholdMemberCommand request, CancellationToken cancellationToken)
    {
        Household? household = await _householdRepository.GetByIdWithMembersAsync(request.HouseholdId, cancellationToken);

        if (household is null)
        {
            return Result.Failure(HouseholdErrors.NotFound);
        }

        if (request.AdminId != household.AdminId)
        {
            return Result.Failure(HouseholdErrors.Restricted);
        }

        User? user = await _userRepository.GetByEmailAsync(request.UserEmail, cancellationToken);

        if (user is null)
        {
            return Result.Failure(UserErrors.NotFound);
        }

        if (household.Members.Any(m => m.UserId == user.Id))
        {
            return Result.Failure(HouseholdErrors.AlreadyMember);
        }

        var newMembership = new HouseholdUser(
            household.Id,
            user.Id,
            UserRole.Member
        );

        household.Members.Add(newMembership);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
