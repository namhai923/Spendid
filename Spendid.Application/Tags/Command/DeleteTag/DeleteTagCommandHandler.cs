using Spendid.Application.Abstractions.Messaging;
using Spendid.Domain.Abstractions;
using Spendid.Domain.Expenses;
using Spendid.Domain.Households;
using Spendid.Domain.Tags;
using Spendid.Domain.Users;

namespace Spendid.Application.Tags.Command.DeleteTag;

internal sealed class DeleteTagCommandHandler(ITagRepository tagRepository, IHouseholdRepository householdRepository, IUnitOfWork unitOfWork) : ICommandHandler<DeleteTagCommand>
{
    private readonly ITagRepository _tagRepository = tagRepository;
    private readonly IHouseholdRepository _householdRepository = householdRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
    {
        var tag = await _tagRepository.GetByIdAsync(request.TagId, cancellationToken);

        if (tag is null)
        {
            return Result.Failure(TagErrors.NotFound);
        }

        var household = await _householdRepository.GetByIdWithMembersAsync(tag.HouseholdId, cancellationToken);

        if (household is null)
        {
            return Result.Failure(HouseholdErrors.NotFound);
        }

        if (!household.Members.Any(m => m.UserId == request.UserId))
        {
            return Result.Failure(HouseholdErrors.MustBeMember);
        }

        _tagRepository.Remove(tag);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
