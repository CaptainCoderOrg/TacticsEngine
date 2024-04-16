namespace CaptainCoder.TacticsEngine.Board;

public abstract record FigureInfo
{
    public static NoFigure None { get; } = new NoFigure();
}
public sealed record NoFigure : FigureInfo;
public sealed record Figure : FigureInfo
{
    public int Width { get; init; } = 1;
    public int Height { get; init; } = 1;
}