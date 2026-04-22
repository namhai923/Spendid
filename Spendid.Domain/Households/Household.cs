using Spendid.Domain.Abstractions;
using Spendid.Domain.Expenses;
using Spendid.Domain.HouseholdUsers;
using Spendid.Domain.Tags;

namespace Spendid.Domain.Households;

public sealed class Household : Entity
{
    private Household(Guid id, HouseholdName householdName, Guid adminId) : base(id)
    {
        HouseholdName = householdName;
        AdminId = adminId;
    }

    private Household()
    {
    }

    public HouseholdName HouseholdName { get; private set; }

    public Guid AdminId { get; private set; }

    public List<Tag> Tags { get; private set; } = [];

    public List<Expense> Expenses { get; private set; } = [];

    public List<HouseholdUser> Members { get; private set; } = [];

    public static Household Create(string householdName, Guid adminId)
    {
        return new(Guid.NewGuid(), new HouseholdName(householdName), adminId);
    }

    public void ChangeName(string newName)
    {
        if (HouseholdName.Value == newName) return;

        HouseholdName = new HouseholdName(newName);
    }

    public void ChangeAdmin(Guid newAdminId)
    {
        if (AdminId == newAdminId) return;

        AdminId = newAdminId;
    }
}
