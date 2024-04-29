using CaptainCoder.TacticsEngine.Board;

namespace WebEditor.Components.BoardEditor;

public abstract class Tool
{
    private ToolManager ToolManager => ToolManager.Shared;
    public virtual void OnSelectTile(Board board, Position position) { }
    public virtual void OnMouseOver(Board board, Position position) { }
    public virtual void OnSelectFigure(Board board, Positioned<Figure> figure)
    {
        ToolManager.Tool = FigureTool.Shared;
        FigureTool.Shared.OnSelectFigure(board, figure);
    }

    public virtual void OnStartDragFigure(Board board, Positioned<Figure> figure, Position offset)
    {
        ToolManager.Tool = FigureTool.Shared;
        FigureTool.Shared.OnStartDragFigure(board, figure, offset);
    }

    public virtual void OnMouseUp(Board board, Position endPosition)
    {
        FigureTool.Shared.OnMouseUp(board, endPosition);
    }
}