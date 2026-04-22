using Spendid.Application.Abstractions.Messaging;
using Spendid.Application.DTOs;

namespace Spendid.Application.Households.Query.GetHousehold;

public record GetHouseholdQuery(Guid HouseholdId) : IQuery<HouseholdDto>;
