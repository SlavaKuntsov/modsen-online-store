using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OnlineStore.API.Converters;

public sealed class EmptyStringToNullableGuidConverter : JsonConverter<Guid?>
{
        public override Guid? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
                if (reader.TokenType == JsonTokenType.String)
                {
                        var value = reader.GetString();
                        if (string.IsNullOrWhiteSpace(value))
                        {
                                return null;
                        }

                        if (Guid.TryParse(value, out var guid))
                        {
                                return guid;
                        }
                }

                if (reader.TokenType == JsonTokenType.Null)
                {
                        return null;
                }

                throw new JsonException($"Unable to convert \"{reader.GetString()}\" to Guid");
        }

        public override void Write(Utf8JsonWriter writer, Guid? value, JsonSerializerOptions options)
        {
                if (value.HasValue)
                {
                        writer.WriteStringValue(value.Value);
                }
                else
                {
                        writer.WriteNullValue();
                }
        }
}
