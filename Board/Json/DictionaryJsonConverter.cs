using System.Text.Json;
using System.Text.Json.Serialization;

namespace CaptainCoder.Json;
public class DictionaryJsonConverter<TKey, TValue> : JsonConverter<Dictionary<TKey, TValue>> where TKey : notnull
{
    public override Dictionary<TKey, TValue>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        JsonSerializerOptions extended = new(options) { IncludeFields = true, };
        (TKey, TValue)[] result = JsonSerializer.Deserialize<(TKey, TValue)[]>(ref reader, extended) ?? throw new JsonException($"Could not parse {typeof(Dictionary<TKey, TValue>)}.");
        return result.ToDictionary();
    }

    public override void Write(Utf8JsonWriter writer, Dictionary<TKey, TValue> value, JsonSerializerOptions options)
    {
        JsonSerializerOptions extended = new(options) { IncludeFields = true, };
        (TKey, TValue)[] serializableArray = [.. value.Select(kvp => (kvp.Key, kvp.Value))];
        JsonSerializer.Serialize(writer, serializableArray, extended);
    }
}