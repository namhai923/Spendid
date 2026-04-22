using Spendid.Application.Abstractions.Authentication;
using Spendid.Domain.Abstractions;
using Spendid.Domain.Users;

namespace Spendid.Infrastructure.Authentication;

internal sealed class AuthenticationService(IUserRepository userRepository, IUnitOfWork unitOfWork) : IAuthenticationService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Guid> GetOrCreateUserAsync(string identityId, string email, string googleName)
    {
        var user = await _userRepository.GetByIdentityIdAsync(identityId);

        if (user is not null)
        {
            if (user.UserName.Value != googleName)
            {
                user.ChangeName(googleName);
            }

            await _unitOfWork.SaveChangesAsync();
            return user.Id;
        }

        var newUser = User.Create(identityId, email, googleName);

        _userRepository.Add(newUser);

        await _unitOfWork.SaveChangesAsync();

        return newUser.Id;
    }
}
