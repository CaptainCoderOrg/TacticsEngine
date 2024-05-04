using Optional;

namespace CaptainCoder.TacticsEngine.Board;

public sealed record Tile
{
    public Option<Figure> Figure { get; init; } = Option.None<Figure>();
}