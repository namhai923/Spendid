using MediatR;
using Spendid.Domain.Abstractions;

namespace Spendid.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
