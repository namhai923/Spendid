namespace Spendid.Domain.Users;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    void Add(User user);

    Task<User?> GetByIdentityIdAsync(string identityId, CancellationToken cancellationToken = default);

    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<List<User>> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken = default);
}
