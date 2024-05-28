using System.Text.Json;
using System.Text.Json.Serialization;
using Webion.Stargaze.Core.Entities;

namespace Webion.Stargaze.Core.Converters;

public sealed class ClickUpObjectIdConverter : JsonConverter<ClickUpObjectId>
{
    public override ClickUpObjectId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is not JsonTokenType.String)
            return default;
        
        var base64Id = reader.GetString();
        if (ClickUpObjectId.TryDeserialize(base64Id, out var id))
            return id;

        throw new JsonException("Could not parse ClickUpObjectId");
    }

    public override void Write(Utf8JsonWriter writer, ClickUpObjectId value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Serialize());
    }
}