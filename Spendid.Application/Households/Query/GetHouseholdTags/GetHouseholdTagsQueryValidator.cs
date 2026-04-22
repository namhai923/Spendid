using FluentValidation;

namespace Spendid.Application.Households.Query.GetHouseholdTags;

public class GetHouseholdTagsQueryValidator : AbstractValidator<GetHouseholdTagsQuery>
{
    public GetHouseholdTagsQueryValidator()
    {
        RuleFor(q => q.HouseholdId).NotEmpty();
    }
}
