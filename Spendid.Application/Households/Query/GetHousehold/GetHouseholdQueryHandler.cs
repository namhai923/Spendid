using Dapper;
using Spendid.Application.Abstractions.Data;
using Spendid.Application.Abstractions.Messaging;
using Spendid.Application.DTOs;
using Spendid.Domain.Abstractions;

namespace Spendid.Application.Households.Query.GetHousehold;

internal sealed class GetHouseholdQueryHandler(ISqlConnectionFactory sqlConnectionFactory) : IQueryHandler<GetHouseholdQuery, HouseholdDto>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory = sqlConnectionFactory;
    public async Task<Result<HouseholdDto>> Handle(GetHouseholdQuery request, CancellationToken cancellationToken)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        var sql = """
            SELECT
                h.id AS Id,
                h.household_name AS HouseholdName,
                jsonb_build_object(
                    'UserEmail', u.email, 
                    'UserName', u.user_name
                ) AS AdminInfo
            FROM households AS h
            LEFT JOIN users AS u ON h.admin_id = u.id
            WHERE h.id = @HouseholdId
            """;

        var household = await connection.QueryFirstOrDefaultAsync<HouseholdDto>(sql, new { request.HouseholdId });

        return household;
    }
}
