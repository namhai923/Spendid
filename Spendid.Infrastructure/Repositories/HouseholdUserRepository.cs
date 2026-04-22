using Microsoft.EntityFrameworkCore;
using Spendid.Domain.HouseholdUsers;

namespace Spendid.Infrastructure.Repositories;

internal sealed class HouseholdUserRepository(ApplicationDbContext dbContext) : Repository<HouseholdUser>(dbContext), IHouseholdUserRepository
{
    public async Task<HouseholdUser?> GetByHouseholdAndUserAsync(
    Guid householdId,
    Guid userId,
    CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<HouseholdUser>()
            .FirstOrDefaultAsync(hu =>
                hu.HouseholdId == householdId &&
                hu.UserId == userId,
                cancellationToken);
    }
}
