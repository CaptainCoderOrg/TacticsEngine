using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CaptainCoder.TacticsEngine.Board;

[JsonDerivedType(typeof(NoFigure), typeDiscriminator: "NoFigure")]
[JsonDerivedType(typeof(Figure), typeDiscriminator: "Figure")]
public abstract record FigureInfo
{
    public static NoFigure None { get; } = new NoFigure();
}
public sealed record NoFigure : FigureInfo;

public sealed record Figure(int Width = 1, int Height = 1) : FigureInfo, IHasSize;


public static class FigureInfoExtensions
{
    public static string ToJson(this FigureInfo info)
    {
        return JsonSerializer.Serialize(info);
    }
    public static bool TryFromJson(string json, [NotNullWhen(true)] out FigureInfo? info)
    {
        info = JsonSerializer.Deserialize<FigureInfo>(json);
        return info is not null;
    }
}