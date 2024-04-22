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
        JsonSerializer.Serialize(writer, value.ToSerializableArray(), extended);
    }
}

public static class DictionaryExtensions
{
    public static (TKey, TValue)[] ToSerializableArray<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> toSerialize) =>
        [.. toSerialize.Select(kvp => (kvp.Key, kvp.Value))];

    public static IEnumerable<KeyValuePair<TKey, TValue>> ToKevValuePairs<TKey, TValue>(this (TKey, TValue)[] toDeserialize) =>
        toDeserialize.Select(pair => new KeyValuePair<TKey, TValue>(pair.Item1, pair.Item2));
}