using Microsoft.EntityFrameworkCore;
using Spendid.Domain.Households;

namespace Spendid.Infrastructure.Repositories;

internal sealed class HouseholdRepository(ApplicationDbContext dbContext) : Repository<Household>(dbContext), IHouseholdRepository
{
    public async Task<Household?> GetByIdWithMembersAsync(Guid id, CancellationToken cancellationToken)
    {
        return await DbContext.Set<Household>()
            .Include(h => h.Members)
            .FirstOrDefaultAsync(h => h.Id == id, cancellationToken);
    }

    public void Remove(Household household)
    {
        DbContext.Remove(household);
    }
}
