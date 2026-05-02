using Spendid.Application.Abstractions.Messaging;

namespace Spendid.Application.Households.Command.ChangeHouseholdName;

public record ChangeHouseholdNameCommand(Guid AdminId, Guid HouseholdId, string HouseholdName) : ICommand;
