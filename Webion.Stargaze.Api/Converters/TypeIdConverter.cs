using System.Text.Json;
using System.Text.Json.Serialization;
using FastIDs.TypeId;

namespace Webion.Stargaze.Api.Converters;

public sealed class TypeIdConverter : JsonConverter<TypeId>
{
    public override TypeId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var id = reader.GetString();
        if (id is null)
            return default;
        
        return TypeId.Parse(id);
    }

    public override void Write(Utf8JsonWriter writer, TypeId value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}