namespace Spendid.Domain.HouseholdUsers;

public interface IHouseholdUserRepository
{
    Task<HouseholdUser?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    void Add(HouseholdUser householdUser);

    Task<HouseholdUser?> GetByHouseholdAndUserAsync(Guid householdId, Guid userId, CancellationToken cancellationToken = default);
}
