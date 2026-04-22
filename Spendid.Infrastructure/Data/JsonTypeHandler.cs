using Dapper;
using System.Data;
using System.Text.Json;

namespace Spendid.Infrastructure.Data;

internal sealed class JsonTypeHandler<T> : SqlMapper.TypeHandler<T>
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public override void SetValue(IDbDataParameter parameter, T? value)
    {
        parameter.Value = value is null
            ? DBNull.Value
            : JsonSerializer.Serialize(value, Options);
    }

    public override T? Parse(object value)
    {
        if (value is null || value is DBNull)
            return default;

        return JsonSerializer.Deserialize<T>(value.ToString()!, Options);
    }
}
