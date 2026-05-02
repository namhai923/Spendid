using FluentAssertions;
using Spendid.Domain.Households;

namespace Spendid.Domain.UnitTests.Households;

public class HouseholdTests
{
    [Fact]
    public void Create_Should_SetPropertyValues()
    {
        // Act
        var household = Household.Create(HouseholdData.HouseholdName, HouseholdData.AdminId);

        // Assert
        household.HouseholdName.Should().Be(new HouseholdName(HouseholdData.HouseholdName));
        household.AdminId.Should().Be(HouseholdData.AdminId);
    }
}
