using FluentValidation;

namespace Spendid.Application.Users.Query.GetMemberships;

internal sealed class GetMembershipsQueryValidator : AbstractValidator<GetMembershipsQuery>
{
    public GetMembershipsQueryValidator()
    {
        RuleFor(q => q.UserId).NotEmpty();
    }
}
