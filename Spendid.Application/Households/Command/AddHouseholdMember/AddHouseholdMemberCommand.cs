using Spendid.Application.Abstractions.Messaging;

namespace Spendid.Application.Households.Command.AddHouseholdMember;

public record AddHouseholdMemberCommand(Guid AdminId, Guid HouseholdId, string UserEmail) : ICommand;
