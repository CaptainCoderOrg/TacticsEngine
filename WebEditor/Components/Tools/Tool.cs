using CaptainCoder.TacticsEngine.Board;

namespace WebEditor.Tools;

public abstract class Tool
{
    private ToolManager ToolManager => ToolManager.Shared;
    public virtual EventResult OnClick(Board board, Position position)
    {
        Console.WriteLine("Click");
        if (board.TryGetTile(position, out Tile? tile) && tile.Figure is Positioned<Figure> figure)
        {
            ToolManager.Tool = FigureTool.Shared;
            FigureTool.Shared.Selected = figure;
            Console.WriteLine($"Selected Figure: {figure}");
            return EventResult.Handled;
        }
        return EventResult.Unhandled;
    }

    public virtual void OnStartDragFigure(Board board, Figure figure, Position? originalPosition = null)
    {
        ToolManager.Tool = DragFigureTool.Shared;
        DragFigureTool.Shared.OnStartDragFigure(board, figure, originalPosition);
    }
    public virtual void OnMouseOver(Board board, Position position) { }
    public virtual void OnMouseUp(Board board, Position endPosition) { }
}