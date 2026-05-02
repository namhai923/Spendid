using Spendid.Application.Abstractions.Messaging;

namespace Spendid.Application.Tags.Command.UpdateTag;

public record UpdateTagCommand(Guid UserId, Guid TagId, string TagName, string Color) : ICommand;
