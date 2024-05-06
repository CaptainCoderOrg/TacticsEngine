using CaptainCoder.TacticsEngine.Board;

namespace WebEditor.Tools;

public abstract class Tool
{
    private ToolManager ToolManager => ToolManager.Shared;
    public virtual EventResult OnClick(Board board, Position position)
    {
        if (board.TryGetTile(position, out Tile? tile) && tile.Figure is Positioned<Figure> figure)
        {
            ToolManager.Tool = FigureTool.Shared;
            FigureTool.Shared.Selected = figure;
            return EventResult.Handled;
        }
        return EventResult.Unhandled;
    }

    public virtual void OnStartDrag(Board board, Position position)
    {
        DragFigureTool.Shared.OnStartDrag(board, position);
    }
    public virtual void OnMouseOver(Board board, Position position) { }
    public virtual void OnMouseUp(Board board, Position endPosition) { }
}