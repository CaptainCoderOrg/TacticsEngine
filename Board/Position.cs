namespace CaptainCoder.TacticsEngine.Board;
public record struct Position(int X, int Y)
{
    public static Position operator +(Position a, Position b) => new(a.X + b.X, a.Y + b.Y);
    public static Position operator -(Position a, Position b) => new(a.X - b.X, a.Y - b.Y);
}