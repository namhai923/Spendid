using Microsoft.EntityFrameworkCore;
using Spendid.Domain.Users;

namespace Spendid.Infrastructure.Repositories;

internal sealed class UserRepository(ApplicationDbContext dbContext) : Repository<User>(dbContext) , IUserRepository
{
    public async Task<User?> GetByIdentityIdAsync(string identityId, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<User>().FirstOrDefaultAsync(entity => entity.IdentityId == new IdentityId(identityId), cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<User>().FirstOrDefaultAsync(entity => entity.Email == new Domain.Users.Email(email), cancellationToken);
    }

    public Task<List<User>> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
