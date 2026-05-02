using Microsoft.EntityFrameworkCore;
using Spendid.Domain.Expenses;

namespace Spendid.Infrastructure.Repositories;

internal sealed class ExpenseRepository(ApplicationDbContext dbContext) : Repository<Expense>(dbContext), IExpenseRepository
{
    public override async Task<Expense?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await DbContext.Set<Expense>()
            .Include(e => e.Tags)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }
    public void Remove(Expense expense)
    {
        DbContext.Remove(expense);
    }
}
