using Spendid.Domain.Abstractions;
using Spendid.Domain.Expenses;

namespace Spendid.Domain.Tags;

public sealed partial class Tag : Entity
{
    private Tag(Guid id, Guid householdId, TagName tagName, string color) : base(id)
    {
        HouseholdId = householdId;
        TagName = tagName;
        Color = color;
    }

    private Tag()
    {
    }

    public Guid HouseholdId { get; private set; }

    public TagName TagName { get; private set; }

    public string Color { get; private set; }

    public List<Expense> Expenses { get; private set; } = [];

    public static Tag Create(Guid householdId, string tagName, string color)
    {
        return new Tag(Guid.NewGuid(), householdId, new TagName(tagName), color.ToUpper());
    }

    public void Update(string newColor, string newName)
    {
        Color = newColor.ToUpper();
        TagName = new TagName(newName);
    }
}
