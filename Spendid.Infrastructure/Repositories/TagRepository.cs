using Microsoft.EntityFrameworkCore;
using Spendid.Domain.Tags;

namespace Spendid.Infrastructure.Repositories;

internal sealed class TagRepository(ApplicationDbContext dbContext) : Repository<Tag>(dbContext), ITagRepository
{
    public async Task<List<Tag>> GetByIdsAsync(List<Guid> ids, Guid householdId, CancellationToken cancellationToken = default)
    {
        var tags = await DbContext.Set<Tag>()
            .Where(tag => ids.Contains(tag.Id) && tag.HouseholdId == householdId)
            .ToListAsync(cancellationToken);

        return tags;
    }

    public void Remove(Tag tag)
    {
        DbContext.Remove(tag);
    }
}
