using System.Text.Json;
using System.Text.Json.Serialization;
using Webion.ClickUp.Api.Common;

namespace Webion.ClickUp.Api.Converters;

public sealed class ClickUpIdConverter : JsonConverter<ClickUpId>
{
    public override ClickUpId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.Number => reader.GetInt64(),
            JsonTokenType.String => reader.GetString(),
            _ => 0
        };
    }

    public override void Write(Utf8JsonWriter writer, ClickUpId value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}