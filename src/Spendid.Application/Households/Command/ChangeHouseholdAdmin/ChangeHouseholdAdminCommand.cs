using Spendid.Application.Abstractions.Messaging;

namespace Spendid.Application.Households.Command.ChangeHouseholdAdmin;

public record ChangeHouseholdAdminCommand(Guid AdminId, Guid HouseholdId, string UserEmail) : ICommand;
