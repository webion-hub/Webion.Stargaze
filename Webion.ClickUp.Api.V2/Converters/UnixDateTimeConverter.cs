using System.Text.Json;
using System.Text.Json.Serialization;

namespace Webion.ClickUp.Api.V2.Converters;

public sealed class UnixDateTimeConverter : JsonConverter<DateTimeOffset>
{
    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var msRaw = reader.GetString();
        if (msRaw is null)
            return default;
        
        var ms = long.Parse(msRaw);
        return DateTimeOffset.FromUnixTimeMilliseconds(ms);
    }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("O"));
    }
}