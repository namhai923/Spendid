using Spendid.Application.Abstractions.Messaging;

namespace Spendid.Application.Users.Query.GetMemberships;

public record GetMembershipsQuery(Guid UserId) : IQuery<IEnumerable<GetMembershipQueryResponse>>;
