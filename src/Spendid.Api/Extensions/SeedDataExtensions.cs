using Bogus;
using Dapper;
using Spendid.Application.Abstractions.Data;
using Spendid.Domain.HouseholdUsers;
using Spendid.Domain.Shared;
using System.Numerics;

namespace Spendid.Api.Extensions;


public static class SeedDataExtensions
{
    private record UserSeed(Guid Id, string IdentityId, string Email, string UserName);

    private record HouseholdSeed(Guid Id, string HouseholdName, Guid AdminId);

    private record HouseholdUserSeed(Guid Id, Guid HouseholdId, Guid UserId, UserRole Role);

    private record ExpenseSeed(Guid Id, Guid HouseholdId, Guid UserId, decimal Amount, string Currency, string Description, DateTime CreatedAt);

    private record TagSeed(Guid Id, Guid HouseholdId, string TagName, string Color);


    public static void SeedData(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var sqlConnectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();
        using var connection = sqlConnectionFactory.CreateConnection();

        var faker = new Faker();

        List<UserSeed> users = [.. Enumerable.Range(0, 19).Select(i => new UserSeed
            (
                Guid.NewGuid(),
                (BigInteger.Parse("0") + i + 1).ToString(),
                faker.Internet.Email(uniqueSuffix: faker.UniqueIndex.ToString()),
                faker.Internet.UserName()
            ))];



        const string usersSql = """
            INSERT INTO public.users
            (id, identity_id, user_name, email)
            VALUES(@Id, @IdentityId, @UserName, @Email);
            """;

        connection.Execute(usersSql, users);

        List<HouseholdSeed> households = [.. Enumerable.Range(0, 30).Select(_ => new HouseholdSeed
            (
                Guid.NewGuid(),
                faker.Address.City() + " Home",
                users[faker.Random.Int(0, users.Count - 1)].Id
            ))];

        const string householdsSql = """
            INSERT INTO public.households
            (id, household_name, admin_id)
            VALUES(@Id, @HouseholdName, @AdminId);
            """;

        connection.Execute(householdsSql, households);

        var memberships = new List<HouseholdUserSeed>();
        var existingPairs = new HashSet<(Guid, Guid)>();

        foreach (var h in households)
        {
            memberships.Add(new HouseholdUserSeed(Guid.NewGuid(), h.Id, h.AdminId, UserRole.Admin));
            existingPairs.Add((h.Id, h.AdminId));
        }

        for (var i = 0; i < 100; i++)
        {
            var randomUser = faker.PickRandom(users).Id;
            var randomHouse = faker.PickRandom(households).Id;

            if (!existingPairs.Contains((randomHouse, randomUser)))
            {
                memberships.Add(new HouseholdUserSeed(Guid.NewGuid(), randomHouse, randomUser, UserRole.Member));
                existingPairs.Add((randomHouse, randomUser));
            }
        }

        const string householdUsersSql = """
            INSERT INTO public.household_users
            (id, household_id, user_id, role)
            VALUES(@Id, @HouseholdId, @UserId, @Role);
            """;

        connection.Execute(householdUsersSql, memberships);

        List<TagSeed> tags = [.. households.SelectMany(h => Enumerable.Range(0, 5).Select(_ => new TagSeed(
                Guid.NewGuid(),
                h.Id,
                faker.Commerce.Categories(1)[0],
                faker.Internet.Color().ToUpper()
            )
        ))];

        const string tagsSql = """
            INSERT INTO public.tags
            (id, household_id, tag_name, color)
            VALUES(@Id, @HouseholdId, @TagName, @Color);
            """;

        connection.Execute(tagsSql, tags);

        List<ExpenseSeed> expenses = [.. memberships.SelectMany(m => Enumerable.Range(0, 8).Select(_ => new ExpenseSeed(
                Guid.NewGuid(),
                m.HouseholdId,
                m.UserId,
                faker.Random.Decimal(5, 1000),
                faker.PickRandom(Currency.All.Select(c => c.Code).ToArray()),
                faker.Commerce.ProductName(),
                faker.Date.Past(1)
            )))];

        const string expensesSql = """
            INSERT INTO public.expenses
            (id, household_id, user_id, amount_amount, amount_currency, description, created_at)
            VALUES(@Id, @HouseholdId, @USerId, @Amount, @Currency, @Description, @CreatedAt);
            """;

        connection.Execute(expensesSql, expenses);

        var expenseTagLinks = expenses.SelectMany(e => {
            var possibleTags = tags.Where(t => t.HouseholdId == e.HouseholdId).ToList();
            return possibleTags.Any()
                ? [new { expensesId = e.Id, tagsId = faker.PickRandom(possibleTags).Id }]
                : Array.Empty<object>();
        }).ToList();

        const string expenseTagsSql = """
            INSERT INTO public.expense_tags
            (expenses_id, tags_id)
            VALUES(@expensesId, @tagsId);
            """;

        connection.Execute(expenseTagsSql, expenseTagLinks);
    }
}
