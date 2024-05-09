using CaptainCoder.TacticsEngine.Board;

namespace WebEditor.Components.DragAndDrop;

public record PositionedFigureDragData(Positioned<Figure> Figure, Position Offset, BoardRenderer Parent) : IDragData
{
    private bool _wasMoved = false;

    public bool CanDrop(BoardData board, Position position)
    {
        return board.CanAddFigure(position + Offset, Figure.Element);
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

    public void HandleDragEnterTile(BoardData board, Position position)
    {
        DragAndDropManager.Shared.DraggedFigure = Figure with { Position = position + Offset };
    }

    public void HandleDragStart() { }

    public void HandleDropTile(BoardData board, Position position)
    {
        _wasMoved = board.TryAddFigure(Figure with { Position = position + Offset });
    }
}