using FluentValidation;

namespace Spendid.Application.Households.Query.GetHouseholdTags;

internal sealed class GetHouseholdTagsQueryValidator : AbstractValidator<GetHouseholdTagsQuery>
{
    public GetHouseholdTagsQueryValidator()
    {
        RuleFor(q => q.HouseholdId).NotEmpty();
    }
}
