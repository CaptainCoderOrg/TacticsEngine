using CaptainCoder.TacticsEngine.Board;

namespace WebEditor.Components.BoardEditor;

public abstract class Tool
{
    private ToolManager ToolManager => ToolManager.Shared;
    public virtual void OnClickTile(Board board, Position position) { }
    public virtual void OnClickFigure(Board board, Positioned<Figure> figure) { }
    public virtual void OnStartDragFigure(Board board, Positioned<Figure> figure, Position offset)
    {
        ToolManager.Tool = FigureTool.Shared;
        FigureTool.Shared.OnStartDragFigure(board, figure, offset);
    }
    public virtual void OnMouseOver(Board board, Position position) { }
    public virtual void OnMouseUp(Board board, Position endPosition) { }
}