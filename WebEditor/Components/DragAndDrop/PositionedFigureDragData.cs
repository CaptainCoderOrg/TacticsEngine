using CaptainCoder.TacticsEngine.Board;

#pragma warning disable IDE0005 // This import is required but dotnet format removes it
using WebEditor.Components.Board;
using WebEditor.Tools;
#pragma warning restore IDE0005 // This import is required but dotnet format removes it
namespace WebEditor.Components.DragAndDrop;

public sealed record PositionedFigureDragData(Positioned<Figure> Figure, Position Offset, BoardRenderer Parent) : IDragData
{
    public void HandleDragStart()
    {
        FigureTool.Shared.Selected = Figure;
    }

    public void HandleDragOverTile(BoardData board, Position position)
    {
        DragAndDropManager.Shared.DraggedFigure = Figure with { Position = position + Offset };
    }

    public bool CanDrop(BoardData board, Position position)
    {
        return board.CanMoveFigure(Figure.Position, position);
    }

    public void HandleDropTile(BoardData board, Position position, Action? onSuccess)
    {
        if (board.TryMoveFigure(Figure.Position, position + Offset))
        {
            FigureTool.Shared.Selected = Figure with { Position = position + Offset };
            onSuccess?.Invoke();
        }
    }

    public void HandleDragEnd()
    {
        DragAndDropManager.Shared.DraggedFigure = null;
    }
}