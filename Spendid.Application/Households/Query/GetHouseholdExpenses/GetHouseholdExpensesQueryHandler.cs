using Dapper;
using Spendid.Application.Abstractions.Data;
using Spendid.Application.Abstractions.Messaging;
using Spendid.Application.DTOs;
using Spendid.Domain.Abstractions;
using Spendid.Domain.Households;

namespace Spendid.Application.Households.Query.GetHouseholdExpenses;

internal sealed class GetHouseholdExpensesQueryHandler(ISqlConnectionFactory sqlConnectionFactory, IHouseholdRepository householdRepository) : IQueryHandler<GetHouseholdExpensesQuery, IEnumerable<ExpenseDto>>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory = sqlConnectionFactory;
    private readonly IHouseholdRepository _householdRepository = householdRepository;

    public async Task<Result<IEnumerable<ExpenseDto>>> Handle(GetHouseholdExpensesQuery request, CancellationToken cancellationToken)
    {
        Household? household = await _householdRepository.GetByIdAsync(request.HouseholdId, cancellationToken);

        if (household is null)
        {
            return Result.Failure<IEnumerable<ExpenseDto>>(HouseholdErrors.NotFound);
        }

        using var connection = _sqlConnectionFactory.CreateConnection();

        var sql = """
            SELECT
                e.id AS Id,
                e.amount_amount AS Amount,
                e.amount_currency AS AmountCurrency,
                e.description AS Description,
                e.created_at AS CreatedAt,
                jsonb_build_object(
                    'UserEmail', u.email, 
                    'UserName', u.user_name
                ) AS UserInfo,
                COALESCE(
                    jsonb_agg(
                        jsonb_build_object(
                            'Id', t.id, 
                            'TagName', t.tag_name, 
                            'Color', t.color
                        )
                    ) FILTER (WHERE t.id IS NOT NULL), '[]'::jsonb
                ) AS Tags
            FROM expenses AS e
            LEFT JOIN expense_tags AS et ON e.id = et.expenses_id
            LEFT JOIN tags AS t ON et.tags_id = t.id
            LEFT JOIN users AS u ON e.user_id = u.id
            WHERE e.household_id = @HouseholdId
            GROUP BY e.id, u.id
            """;

        var expenses = await connection.QueryAsync<ExpenseDto>(sql, new { request.HouseholdId });

        return Result.Create(expenses);
    }
}
