using Microsoft.EntityFrameworkCore;
using Spendid.Domain.Abstractions;

namespace Spendid.Infrastructure.Repositories;

internal abstract class Repository<T>(ApplicationDbContext dbContext) where T : Entity
{
    protected readonly ApplicationDbContext DbContext = dbContext;

    public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await DbContext.Set<T>().FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);
    }

    public void Add(T entity)
    {
        DbContext.Add(entity);
    }
}
