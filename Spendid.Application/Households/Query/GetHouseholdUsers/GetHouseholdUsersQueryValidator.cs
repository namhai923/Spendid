using FluentValidation;

namespace Spendid.Application.Households.Query.GetHouseholdUsers;

public class GetHouseholdUsersQueryValidator : AbstractValidator<GetHouseholdUsersQuery>
{
    public GetHouseholdUsersQueryValidator()
    {
        RuleFor(q => q.HouseholdId).NotEmpty();
    }
}
