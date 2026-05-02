using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Spendid.Application.Abstractions.Clock;
using Spendid.Application.Exceptions;
using Spendid.Domain.Abstractions;
using Spendid.Infrastructure.Outbox;
using System.Data;

namespace Spendid.Infrastructure;

public sealed class ApplicationDbContext(DbContextOptions options, IDateTimeProvider dateTimeProvider) : DbContext(options), IUnitOfWork
{
    private static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All,
    };

    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            AddDomainEventsAsOutboxMessages();

            var result = await base.SaveChangesAsync(cancellationToken);

             return result;
        }
        catch (DBConcurrencyException exception)
        {
            throw new ConcurrencyException("Concurrency exception occurred.", exception);
        }
    }

    private void AddDomainEventsAsOutboxMessages()
    {
        var outboxMessages = ChangeTracker
            .Entries<Entity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                var domainEvents = entity.GetDomainEvents();

                entity.ClearDomainEvents();

                return domainEvents;
            })
            .Select(domainEvent => new OutboxMessage(
                        Guid.NewGuid(),
                        _dateTimeProvider.UtcNow,
                        domainEvent.GetType().Name,
                        JsonConvert.SerializeObject(domainEvent, JsonSerializerSettings)
                    )
            )
            .ToList();

        AddRange(outboxMessages);
    }
}
