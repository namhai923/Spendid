using Dapper;
using Spendid.Application.Abstractions.Data;
using Spendid.Application.Abstractions.Messaging;
using Spendid.Application.DTOs;
using Spendid.Domain.Abstractions;
using Spendid.Domain.Households;

namespace Spendid.Application.Households.Query.GetHouseholdUsers;

internal sealed class GetHouseholdUsersQueryHandler(ISqlConnectionFactory sqlConnectionFactory, IHouseholdRepository householdRepository) : IQueryHandler<GetHouseholdUsersQuery, IEnumerable<GetHouseholdUsersQueryResponse>>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory = sqlConnectionFactory;
    private readonly IHouseholdRepository _householdRepository = householdRepository;

    public async Task<Result<IEnumerable<GetHouseholdUsersQueryResponse>>> Handle(GetHouseholdUsersQuery request, CancellationToken cancellationToken)
    {
        Household? household = await _householdRepository.GetByIdAsync(request.HouseholdId, cancellationToken);

        if (household is null)
        {
            return Result.Failure<IEnumerable<GetHouseholdUsersQueryResponse>>(HouseholdErrors.NotFound);
        }

        using var connection = _sqlConnectionFactory.CreateConnection();

        var sql = """
            SELECT 
                u.email AS Email,
                u.user_name AS UserName,
                hu.role AS Role
            FROM users u
            INNER JOIN household_users hu ON u.id = hu.user_id
            WHERE hu.household_id = @HouseholdId
            """;

        var tags = await connection.QueryAsync<GetHouseholdUsersQueryResponse>(sql, new { request.HouseholdId });

        return Result.Create(tags);
    }
}
