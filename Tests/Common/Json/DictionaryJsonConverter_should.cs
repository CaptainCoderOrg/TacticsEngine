namespace CaptainCoder.Json.Tests;

using System.Text.Json;

using Shouldly;

public class BoardDictionaryJsonConverter_should
{
    [Fact]
    public void serialize_and_deserialize_to_json()
    {
        Dictionary<ArbitraryKey, ArbitraryValue> underTest = new()
        {
            { new ArbitraryKey(5, 7), new ArbitraryValue("Hello world") },
            { new ArbitraryKey(2, 1), new ArbitraryValue("Another string") },
        };

        JsonSerializerOptions options = new()
        {
            Converters = { new DictionaryJsonConverter<ArbitraryKey, ArbitraryValue>() },
        };
        string json = JsonSerializer.Serialize(underTest, options);

        Dictionary<ArbitraryKey, ArbitraryValue>? restored = JsonSerializer.Deserialize<Dictionary<ArbitraryKey, ArbitraryValue>>(json, options);
        restored.ShouldNotBeNull();
        restored.Count.ShouldBe(underTest.Count);
        restored[new ArbitraryKey(5, 7)].ShouldBe(new ArbitraryValue("Hello world"));
        restored[new ArbitraryKey(2, 1)].ShouldBe(new ArbitraryValue("Another string"));
    }

    [Fact]
    public void deserialize_null()
    {
        JsonSerializerOptions options = new()
        {
            Converters = { new DictionaryJsonConverter<ArbitraryKey, ArbitraryValue>() },
        };
        ClassWithDictionary? restored = JsonSerializer.Deserialize<ClassWithDictionary>("""{ "ToSave": null }""", options);
        restored.ShouldNotBeNull();
        restored.ToSave.ShouldBeNull();
    }
}

public class ClassWithDictionary
{
    public Dictionary<ArbitraryKey, ArbitraryValue>? ToSave { get; set; }
}

public record ArbitraryKey(int X, int Y);
public record ArbitraryValue(string SomeValue);