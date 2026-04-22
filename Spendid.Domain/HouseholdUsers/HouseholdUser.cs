using Spendid.Domain.Abstractions;
using Spendid.Domain.Households;
using Spendid.Domain.Users;

namespace Spendid.Domain.HouseholdUsers;

public sealed class HouseholdUser : Entity
{
    public HouseholdUser(Guid householdId, Guid userId, UserRole role)
    {
        HouseholdId = householdId;
        UserId = userId;
        Role = role;
    }

    private HouseholdUser()
    {
    }

    public Guid HouseholdId { get; private set; }

    public Household? Household { get; private set; } = null;

    public Guid UserId { get; private set; }

    public User? User { get; private set; } = null;

    public UserRole Role { get; private set; }

    public void UpdateRole(UserRole newRole)
    {
        Role = newRole;
    }
}
