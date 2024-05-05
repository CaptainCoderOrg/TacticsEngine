using CaptainCoder.TacticsEngine.Board;

namespace WebEditor.Tools;

public abstract class Tool
{
    private ToolManager ToolManager => ToolManager.Shared;
    public virtual void OnClickTile(Board board, Position position) { }
    public virtual void OnClickFigure(Board board, Positioned<Figure> figure) { }
    public virtual void OnStartDragFigure(Board board, Figure figure, Position? startDragPosition = null)
    {
        ToolManager.Tool = FigureTool.Shared;
        FigureTool.Shared.OnStartDragFigure(board, figure, startDragPosition);
    }
    public virtual void OnMouseOver(Board board, Position position) { }
    public virtual void OnMouseUp(Board board, Position endPosition) { }
}