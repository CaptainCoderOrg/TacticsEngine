namespace CaptainCoder.TacticsEngine.Board;

public abstract record FigureInfo
{
    public static NoFigure None { get; } = new NoFigure();
}
public record NoFigure : FigureInfo;
public record Figure : FigureInfo
{
    public int Width { get; init; } = 1;
    public int Height { get; init; } = 1;
}