using FluentValidation;

namespace Spendid.Application.Households.Query.GetHouseholdUsers;

internal sealed class GetHouseholdUsersQueryValidator : AbstractValidator<GetHouseholdUsersQuery>
{
    public GetHouseholdUsersQueryValidator()
    {
        RuleFor(q => q.HouseholdId).NotEmpty();
    }
}
