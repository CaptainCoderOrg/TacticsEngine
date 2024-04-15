namespace CaptainCoder.TacticsEngine.Board;

public abstract record Figure
{
    public static NoFigure None { get; } = new NoFigure();
}
public record NoFigure : Figure;