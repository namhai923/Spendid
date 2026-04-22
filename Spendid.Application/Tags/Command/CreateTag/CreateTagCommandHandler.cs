using Spendid.Application.Abstractions.Messaging;
using Spendid.Domain.Abstractions;
using Spendid.Domain.Households;
using Spendid.Domain.Tags;

namespace Spendid.Application.Tags.Command.CreateTag;

internal sealed class CreateTagCommandHandler(IHouseholdRepository householdRepository, ITagRepository tagRepository, IUnitOfWork unitOfWork) : ICommandHandler<CreateTagCommand>
{
    private readonly IHouseholdRepository _householdRepository = householdRepository;
    private readonly ITagRepository _tagRepository = tagRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(CreateTagCommand request, CancellationToken cancellationToken)
    {
        var household = await _householdRepository.GetByIdWithMembersAsync(request.HouseholdId, cancellationToken);

        if (household is null) {
            return Result.Failure(HouseholdErrors.NotFound);
        }

        if (!household.Members.Any(m => m.UserId == request.UserId))
        {
            return Result.Failure(HouseholdErrors.MustBeMember);
        }

        var tag = Tag.Create(
            request.HouseholdId,
            request.TagName,
            request.Color
        );

        _tagRepository.Add(tag);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
