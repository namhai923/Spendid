using Spendid.Application.Abstractions.Messaging;

namespace Spendid.Application.Tags.Command.DeleteTag;

public record DeleteTagCommand(Guid UserId, Guid TagId) : ICommand;
