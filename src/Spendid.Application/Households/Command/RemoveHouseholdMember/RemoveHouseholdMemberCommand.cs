using Spendid.Application.Abstractions.Messaging;

namespace Spendid.Application.Households.Command.RemoveHouseholdMember;

public record RemoveHouseholdMemberCommand(Guid AdminId, Guid HouseholdId, string UserEmail) : ICommand;
