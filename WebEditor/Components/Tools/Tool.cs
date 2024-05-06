using CaptainCoder.TacticsEngine.Board;

namespace WebEditor.Tools;

public abstract class Tool
{
    private ToolManager ToolManager => ToolManager.Shared;
    public virtual void OnClick(Board board, Position position) { }
    public virtual void OnStartDragFigure(Board board, Figure figure, Position? originalPosition = null)
    {
        ToolManager.Tool = FigureTool.Shared;
        FigureTool.Shared.OnStartDragFigure(board, figure, originalPosition);
    }
    public virtual void OnMouseOver(Board board, Position position) { }
    public virtual void OnMouseUp(Board board, Position endPosition) { }
}