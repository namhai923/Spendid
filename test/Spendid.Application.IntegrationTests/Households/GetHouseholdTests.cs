using FluentAssertions;
using Spendid.Application.Households.Query.GetHousehold;
using Spendid.Application.IntegrationTests.Infrastructure;
using Spendid.Domain.Households;

namespace Spendid.Application.IntegrationTests.Households;

public class GetHouseholdTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    private static readonly Guid HouseholdId = Guid.NewGuid();

    [Fact]
    public async Task GetHousehold_ShouldReturnFailure_WhenHouseholdIsNotFound()
    {
        // Arrange
        var query = new GetHouseholdQuery(HouseholdId);

        // Act
        var result = await Sender.Send(query);

        //Assert
        result.Error.Should().Be(HouseholdErrors.NotFound);
    }
}
