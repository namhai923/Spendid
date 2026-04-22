using Spendid.Application.Abstractions.Messaging;
using Spendid.Domain.Abstractions;
using Spendid.Domain.Households;
using Spendid.Domain.HouseholdUsers;
using Spendid.Domain.Users;

namespace Spendid.Application.Households.Command.ChangeHouseholdAdmin;

internal sealed class ChangeHouseholdAdminCommandHandler(IHouseholdRepository householdRepository, IUserRepository userRepository, IHouseholdUserRepository householdUserRepository, IUnitOfWork unitOfWork) : ICommandHandler<ChangeHouseholdAdminCommand>
{
    private readonly IHouseholdRepository _householdRepository = householdRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IHouseholdUserRepository _householdUserRepository = householdUserRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(ChangeHouseholdAdminCommand request, CancellationToken cancellationToken)
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

        if (!household.Members.Any(m => m.UserId == user.Id))
        {
            return Result.Failure(HouseholdErrors.MustBeMember);
        }

        household.ChangeAdmin(user.Id);

        var oldAdminMember = await _householdUserRepository.GetByHouseholdAndUserAsync(
            household.Id, request.AdminId, cancellationToken);

        oldAdminMember?.UpdateRole(UserRole.Member);

        var newAdminMember = await _householdUserRepository.GetByHouseholdAndUserAsync(
            household.Id, user.Id, cancellationToken);

        newAdminMember?.UpdateRole(UserRole.Admin);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
