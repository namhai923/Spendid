using Spendid.Domain.Abstractions;
using Spendid.Domain.Expenses.Events;
using Spendid.Domain.Shared;
using Spendid.Domain.Tags;

namespace Spendid.Domain.Expenses;

public sealed class Expense : Entity
{
    private Expense(Guid id, Guid householdId, Guid userId, Money amount, Description description, List<Tag> tags, DateTime createdAt) : base(id)
    {
        HouseholdId = householdId;
        UserId = userId;
        Amount = amount;
        Description = description;
        Tags = tags;
        CreatedAt = createdAt;
    }

    private Expense()
    {
    }

    public Guid HouseholdId { get; init; }

    public Guid UserId { get; init; }

    public Money Amount { get; private set; }

    public Description Description { get; private set; }

    public List<Tag> Tags { get; private set; } = [];

    public DateTime CreatedAt { get; private set; }

    public static Expense Make(Guid userId, Guid householdId, decimal amount, string amountCurrency, string description, List<Tag> tags, DateTime createdAt)
    {
        var expense = new Expense(
            Guid.NewGuid(),
            householdId,
            userId, new Money(amount, Currency.FromCode(amountCurrency)),
            new Description(description),
            tags,
            createdAt);

        expense.RaiseDomainEvent(new ExpenseMadeDomainEvent(expense.Id));

        return expense;
    }

    public void Update(decimal amount, string amountCurrency, string description, List<Tag> tags)
    {
        Amount = new Money(Math.Round(amount, 2), Currency.FromCode(amountCurrency));
        Description = new Description(description);

        // 1. Remove tags that are no longer present in the new list
        var tagsToRemove = Tags.Where(t => !tags.Any(nt => nt.Id == t.Id)).ToList();
        foreach (var tag in tagsToRemove)
        {
            Tags.Remove(tag);
        }

        // 2. Add tags that aren't already in the collection
        var tagsToAdd = tags.Where(nt => !Tags.Any(t => t.Id == nt.Id)).ToList();
        foreach (var tag in tagsToAdd)
        {
            Tags.Add(tag);
        }
    }
}
