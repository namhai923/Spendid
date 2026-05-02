namespace Spendid.Domain.Tags;

public interface ITagRepository
{
    Task<Tag?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    void Add(Tag tag);

    void Remove(Tag tag);

    Task<List<Tag>> GetByIdsAsync(List<Guid> ids, Guid householdId, CancellationToken cancellationToken = default);
}
