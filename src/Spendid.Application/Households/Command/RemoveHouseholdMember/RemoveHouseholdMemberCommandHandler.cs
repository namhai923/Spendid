using Spendid.Application.Abstractions.Messaging;
using Spendid.Domain.Abstractions;
using Spendid.Domain.Households;
using Spendid.Domain.HouseholdUsers;
using Spendid.Domain.Users;

namespace Spendid.Application.Households.Command.RemoveHouseholdMember;

internal sealed class RemoveHouseholdMemberCommandHandler(IHouseholdRepository householdRepository, IUserRepository userRepository, IUnitOfWork unitOfWork) : ICommandHandler<RemoveHouseholdMemberCommand>
{
    private readonly IHouseholdRepository _householdRepository = householdRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(RemoveHouseholdMemberCommand request, CancellationToken cancellationToken)
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

        var member = household.Members.FirstOrDefault(m => m.UserId == user.Id);

        if (member is null)
        {
            return Result.Failure(HouseholdErrors.MustBeMember);
        }

        if (member.Role == UserRole.Admin)
        {
            return Result.Failure(HouseholdErrors.Restricted);
        }

        household.Members.Remove(member);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
