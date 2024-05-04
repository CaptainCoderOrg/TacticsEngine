using System.Text.Json.Serialization;

using Optional;

namespace CaptainCoder.TacticsEngine.Board;

[JsonDerivedType(typeof(NoTile), typeDiscriminator: "NoTile")]
[JsonDerivedType(typeof(Tile), typeDiscriminator: "Tile")]
public abstract record TileInfo
{
    public static NoTile None { get; } = new NoTile();
    public static Tile Empty { get; } = new();
}
public sealed record NoTile : TileInfo;
public sealed record Tile : TileInfo
{
    public Option<Figure> Figure { get; init; } = Option.None<Figure>();
    public PropInfo Prop { get; init; } = PropInfo.None;
}