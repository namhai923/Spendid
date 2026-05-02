using Dapper;
using Spendid.Application.Abstractions.Data;
using Spendid.Application.Abstractions.Messaging;
using Spendid.Application.DTOs;
using Spendid.Domain.Abstractions;
using Spendid.Domain.Households;

namespace Spendid.Application.Households.Query.GetHouseholdTags;

internal sealed class GetHouseholdTagsQueryHandler(ISqlConnectionFactory sqlConnectionFactory, IHouseholdRepository householdRepository) : IQueryHandler<GetHouseholdTagsQuery, IEnumerable<TagDto>>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory = sqlConnectionFactory;
    private readonly IHouseholdRepository _householdRepository = householdRepository;

    public async Task<Result<IEnumerable<TagDto>>> Handle(GetHouseholdTagsQuery request, CancellationToken cancellationToken)
    {
        Household? household = await _householdRepository.GetByIdAsync(request.HouseholdId, cancellationToken);

        if (household is null)
        {
            return Result.Failure<IEnumerable<TagDto>>(HouseholdErrors.NotFound);
        }

        using var connection = _sqlConnectionFactory.CreateConnection();

        var sql = """
            SELECT
                id AS Id,
                tag_name AS TagName,
                color AS Color
            FROM tags
            WHERE household_id = @HouseholdId
            ORDER BY tag_name ASC
            """;

        var tags = await connection.QueryAsync<TagDto>(sql, new { request.HouseholdId });

        return Result.Create(tags);
    }
}
