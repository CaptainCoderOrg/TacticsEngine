namespace CaptainCoder.TacticsEngine.Board;
public record Position(int X, int Y)
{
    public static Position operator +(Position a, Position b) => new(a.X + b.X, a.Y + b.Y);
}

public record NoPosition() : Position(int.MinValue, int.MinValue);