using Spendid.Application.Abstractions.Messaging;

namespace Spendid.Application.Tags.Command.CreateTag;

public record CreateTagCommand(Guid UserId, Guid HouseholdId, string TagName, string Color) : ICommand;
