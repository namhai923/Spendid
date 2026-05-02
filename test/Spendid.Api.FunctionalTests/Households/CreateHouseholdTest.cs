using FluentAssertions;
using Spendid.Api.Controllers.Households;
using Spendid.Api.FunctionalTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;

namespace Spendid.Api.FunctionalTests.Households;

public class CreateHouseholdTest(FunctionalTestWebAppFactory factory) : BaseFunctionalTest(factory)
{
    [Fact]
    public async Task CreateHousehold_ShouldReturnOk_WhenRequestIsValid()
    {
        // Arrange
        var request = new CreateHouseholdRequest(HouseholdData.HouseholdName);

        // Act
        var response = await httpClient.PostAsJsonAsync("api/v1/households", request);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
