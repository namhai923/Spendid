using FluentValidation;

namespace Spendid.Application.Users.Query.GetMemberships;

public class GetMembershipsQueryValidator : AbstractValidator<GetMembershipsQuery>
{
    public GetMembershipsQueryValidator()
    {
        RuleFor(q => q.UserId).NotEmpty();
    }
}
