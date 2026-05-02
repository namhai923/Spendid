using FluentAssertions;
using NSubstitute;
using Spendid.Application.Households.Command.AddHouseholdMember;
using Spendid.Domain.Abstractions;
using Spendid.Domain.Households;
using Spendid.Domain.Users;

namespace Spendid.Application.UnitTests.Households;

public class AddHouseholdMemberTests
{
    private static readonly AddHouseholdMemberCommand Command = new(
        Guid.NewGuid(), 
        Guid.NewGuid(), 
        "test@gmail.com"
    );

    private readonly AddHouseholdMemberCommandHandler _handler;
    private readonly IHouseholdRepository _householdRepositoryMock;
    private readonly IUserRepository _userRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;

    public AddHouseholdMemberTests()
    {
        _householdRepositoryMock = Substitute.For<IHouseholdRepository>();
        _userRepositoryMock = Substitute.For<IUserRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _handler = new AddHouseholdMemberCommandHandler(
            _householdRepositoryMock,
            _userRepositoryMock,
            _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenHouseholdIsNull()
    {
        // Arrange
        _householdRepositoryMock.GetByIdWithMembersAsync(Command.HouseholdId, Arg.Any<CancellationToken>()).Returns((Household?)null);

        // Act
        var result = await _handler.Handle(Command, default);

        //Assert
        result.Error.Should().Be(HouseholdErrors.NotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenAdminIdNotMatch()
    {
        // Arrange
        var household = HouseholdData.Create();

        _householdRepositoryMock.GetByIdWithMembersAsync(Command.HouseholdId, Arg.Any<CancellationToken>()).Returns(household);

        // Act
        var result = await _handler.Handle(Command, default);

        //Assert
        result.Error.Should().Be(HouseholdErrors.Restricted);
    }
}
 