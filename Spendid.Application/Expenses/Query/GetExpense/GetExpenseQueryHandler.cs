using Dapper;
using Spendid.Application.Abstractions.Data;
using Spendid.Application.Abstractions.Messaging;
using Spendid.Application.DTOs;
using Spendid.Domain.Abstractions;

namespace Spendid.Application.Expenses.Query.GetExpense;

internal sealed class GetExpenseQueryHandler(ISqlConnectionFactory sqlConnectionFactory) : IQueryHandler<GetExpenseQuery, ExpenseDto>
{

    private readonly ISqlConnectionFactory _sqlConnectionFactory = sqlConnectionFactory;

    public async Task<Result<ExpenseDto>> Handle(GetExpenseQuery request, CancellationToken cancellationToken)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        const string sql = """
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
                    ) FILTER (WHERE t.id IS NOT NULL), '[]'
                ) AS Tags
            FROM expenses AS e
            LEFT JOIN expense_tags AS et ON e.id = et.expenses_id
            LEFT JOIN tags AS t ON et.tags_id = t.id
            LEFT JOIN users AS u ON e.user_id = u.id
            WHERE e.id = @ExpenseId
            GROUP BY e.id, u.id
            """;

        var expense = await connection.QueryFirstOrDefaultAsync<ExpenseDto>(sql, new { request.ExpenseId });

        return expense;
    }
}
