namespace Spendid.Api.Controllers.Tags;

public sealed record CreateTagRequest(Guid HouseholdId, string TagName, string Color);
