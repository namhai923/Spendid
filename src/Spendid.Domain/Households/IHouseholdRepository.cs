namespace Spendid.Domain.Households;

public interface IHouseholdRepository
{
    Task<Household?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    void Add(Household household);

    void Remove(Household household);

    Task<Household?> GetByIdWithMembersAsync(Guid id, CancellationToken cancellationToken = default);
}
