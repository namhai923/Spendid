using Spendid.Application.Abstractions.Messaging;
using Spendid.Application.DTOs;

namespace Spendid.Application.Households.Query.GetHouseholdTags;

public record GetHouseholdTagsQuery(Guid HouseholdId) : IQuery<IEnumerable<TagDto>>;
