using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

using Newtonsoft.Json;

namespace CaptainCoder.TacticsEngine.Board;

public abstract record FigureInfo
{
    public static NoFigure None { get; } = new NoFigure();
}
public sealed record NoFigure : FigureInfo;

// Figure { Width = int, Height = int }
public sealed record Figure : FigureInfo
{
    public int Width { get; init; } = 1;
    public int Height { get; init; } = 1;
}

public class FigureInfoConverter : JsonConverter<FigureInfo>
{
    public static FigureInfoConverter Shared { get; } = new();
    public override FigureInfo? ReadJson(JsonReader reader, Type objectType, FigureInfo? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        string toParse = reader.Value as string ?? throw new JsonException($"Could not parse {nameof(FigureInfo)} from {reader.Value}.");
        bool success = FigureInfoExtensions.TryParseFigureInfo(toParse, out FigureInfo? result);
        if (success is false) { throw new JsonException($"Could not parse {nameof(FigureInfo)} from {reader.Value}."); }
        return result;
        // (TKey, TValue)[] elems = serializer.Deserialize<(TKey, TValue)[]>(reader) ?? throw new Exception($"Could not parse json to Dictionary<{typeof(TKey)}, {typeof(TValue)}.");
        // return new Dictionary<TKey, TValue>(elems.ToKevValuePairs());
    }

    public override void WriteJson(JsonWriter writer, FigureInfo? value, JsonSerializer serializer)
    {
        if (value is null) { throw new InvalidOperationException($"Cannot write null FigureInfo."); }
        serializer.Serialize(writer, value.ToString());
    }
}

public static class FigureInfoExtensions
{
    public static string ToJson(this FigureInfo info)
    {
        JsonSerializerSettings settings = new();
        settings.Converters = [FigureInfoConverter.Shared];
        return JsonConvert.SerializeObject(info, settings);
    }
    public static bool TryFromJson(string json, [NotNullWhen(true)] out FigureInfo? info)
    {
        JsonSerializerSettings settings = new();
        settings.Converters = [FigureInfoConverter.Shared];
        info = JsonConvert.DeserializeObject<FigureInfo>(json, settings);
        return info is not null;
    }

    public static bool TryParseFigureInfo(string toParse, [NotNullWhen(true)] out FigureInfo? info)
    {
        info = null;
        if (TryParseNoFigure(toParse, out NoFigure? noFigure))
        {
            info = noFigure;
            return true;
        }
        if (TryParseFigure(toParse, out Figure? figure))
        {
            info = figure;
            return true;
        }
        return false;
    }

    private static bool TryParseNoFigure(string toParse, [NotNullWhen(true)] out NoFigure? noFigure)
    {
        noFigure = null;
        if (toParse == "NoFigure { }")
        {
            noFigure = FigureInfo.None;
        }
        return noFigure is not null;
    }

    private static Regex FigureRegex { get; } = new(@"Figure { Width = (?<width>\d+), Height = (?<height>\d+) }");

    private static bool TryParseFigure(string toParse, [NotNullWhen(true)] out Figure? figure)
    {
        figure = null;
        Match match = FigureRegex.Match(toParse);
        if (!match.Success)
        {
            return false;
        }
        figure = new Figure() { Width = int.Parse(match.Groups["width"].Value), Height = int.Parse(match.Groups["height"].Value) };
        return true;
    }
}