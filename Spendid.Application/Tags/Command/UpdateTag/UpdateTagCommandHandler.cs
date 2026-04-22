using Spendid.Application.Abstractions.Messaging;
using Spendid.Domain.Abstractions;
using Spendid.Domain.Households;
using Spendid.Domain.Tags;

namespace Spendid.Application.Tags.Command.UpdateTag;

internal sealed class UpdateTagCommandHandler(ITagRepository tagRepository, IHouseholdRepository householdRepository, IUnitOfWork unitOfWork) : ICommandHandler<UpdateTagCommand>
{
    private readonly ITagRepository _tagRepository = tagRepository;
    private readonly IHouseholdRepository _householdRepository = householdRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
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

        tag.Update(request.Color, request.TagName);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();

    }
}
