using Dapper;
using Spendid.Application.Abstractions.Data;
using Spendid.Application.Abstractions.Messaging;
using Spendid.Domain.Abstractions;

namespace Spendid.Application.Users.Query.GetMemberships;

internal sealed class GetMembershipsQueryHandler(ISqlConnectionFactory sqlConnectionFactory) : IQueryHandler<GetMembershipsQuery, IEnumerable<GetMembershipQueryResponse>>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory = sqlConnectionFactory;

    public async Task<Result<IEnumerable<GetMembershipQueryResponse>>> Handle(GetMembershipsQuery request, CancellationToken cancellationToken)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        var sql = """
            SELECT 
                hu.household_id AS HouseholdId, 
                h.household_name AS HouseholdName,
                hu.role AS Role
            FROM household_users AS hu
            JOIN 
                households AS h
            ON
                hu.household_id = h.id
            WHERE 
                hu.user_id = @UserId
            """;

        var memberships = await connection.QueryAsync<GetMembershipQueryResponse>(sql, new { request.UserId });

        return Result.Create(memberships);
    }
}
