using Spendid.Application.Abstractions.Messaging;

namespace Spendid.Application.Households.Command.DeleteHousehold;

public record DeleteHouseholdCommand(Guid UserId, Guid HouseholdId) : ICommand;
