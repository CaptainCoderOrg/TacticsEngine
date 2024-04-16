namespace CaptainCoder.TacticsEngine.Board;

public abstract record TileInfo
{
    public static NoTile None { get; } = new NoTile();
    public static Tile Empty { get; } = new();
}
public sealed record NoTile : TileInfo;
public sealed record Tile : TileInfo
{
    public FigureInfo Figure { get; init; } = FigureInfo.None;
    public PropInfo Prop { get; init; } = PropInfo.None;
}