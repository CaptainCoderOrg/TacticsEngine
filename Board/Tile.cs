namespace CaptainCoder.TacticsEngine.Board;

public abstract record Tile
{
    public static NoTile None { get; } = new NoTile();
    public static TileInfo Empty { get; } = new();
}
public record NoTile : Tile;
public record TileInfo : Tile
{
    public Figure Figure { get; init; } = Figure.None;
    public Prop Prop { get; init; } = Prop.None;
}