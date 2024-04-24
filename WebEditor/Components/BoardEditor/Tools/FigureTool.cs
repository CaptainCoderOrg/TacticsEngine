using CaptainCoder.TacticsEngine.Board;

namespace WebEditor.Components.BoardEditor;
public sealed class FigureTool : Tool
{
    public static FigureTool Shared { get; } = new();
    public Figure ToDraw { get; private set; } = new() { Width = 1, Height = 2 };
    public override void OnClick(Board board, Position position)
    {
        board.AddFigure(position.X, position.Y, ToDraw);
    }
}