using CaptainCoder.TacticsEngine.Board;

#pragma warning disable IDE0005 // This import is required but dotnet format removes it
using WebEditor.Components.Board;
using WebEditor.Tools;
#pragma warning restore IDE0005 // This import is required but dotnet format removes it
namespace WebEditor.Components.DragAndDrop;

public sealed record PositionedFigureDragData(Positioned<Figure> Figure, Position Offset, BoardRenderer Parent) : IDragData
{
    private bool _wasMoved = false;

    public void HandleDragStart()
    {
        FigureTool.Shared.Selected = Figure;
    }

    public void HandleDragEnterTile(BoardData board, Position position)
    {
        DragAndDropManager.Shared.DraggedFigure = Figure with { Position = position + Offset };
    }

    public bool CanDrop(BoardData board, Position position)
    {
        return board.CanAddFigure(position, Figure.Element);
    }

    public void HandleDropTile(BoardData board, Position position)
    {
        var toMove = Figure with { Position = position + Offset };
        if (board.TryAddFigure(toMove))
        {
            _wasMoved = true;
            FigureTool.Shared.Selected = toMove;
        }
    }

    public void HandleDragEnd()
    {
        DragAndDropManager.Shared.DraggedFigure = null;
        if (_wasMoved)
        {
            _ = Parent.Board.TryRemoveFigure(Figure.Position, out var _);
        }
        Parent.Redraw();
    }
}