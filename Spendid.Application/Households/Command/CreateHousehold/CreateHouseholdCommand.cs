using Spendid.Application.Abstractions.Messaging;

namespace Spendid.Application.Households.Command.CreateHousehold;

public record CreateHouseholdCommand(Guid UserId, string HouseholdName) : ICommand;
