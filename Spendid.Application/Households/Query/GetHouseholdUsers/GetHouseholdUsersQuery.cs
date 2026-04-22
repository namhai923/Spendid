using Spendid.Application.Abstractions.Messaging;

namespace Spendid.Application.Households.Query.GetHouseholdUsers;

public record GetHouseholdUsersQuery(Guid HouseholdId) : IQuery<IEnumerable<GetHouseholdUsersQueryResponse>>;
