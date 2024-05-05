namespace CaptainCoder.TacticsEngine.Board;
public sealed record Tile
{
    public Positioned<Figure>? Figure { get; init; }
}