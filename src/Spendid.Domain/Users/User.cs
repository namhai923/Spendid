using Spendid.Domain.Abstractions;
using Spendid.Domain.HouseholdUsers;

namespace Spendid.Domain.Users;

public sealed class User : Entity
{

    private User(Guid id, IdentityId identityId, Email email, UserName userName) : base(id)
    {
        IdentityId = identityId;
        Email = email;
        UserName = userName;
    }

    private User()
    {
    }

    public IdentityId IdentityId { get; private set; }

    public Email Email { get; private set; }

    public UserName UserName { get; private set; }

    public List<HouseholdUser> HouseholdMemberships { get; private set; } = [];

    public static User Create(string identityId, string email, string userName)
    {
        var user = new User(Guid.NewGuid(), new IdentityId(identityId), new Email(email), new UserName(userName));

        return user;
    }

    public void ChangeName(string newName)
    {
        if (UserName.Value == newName) return;

        UserName = new UserName(newName);
    }
}
