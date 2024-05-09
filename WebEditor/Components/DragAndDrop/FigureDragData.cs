using CaptainCoder.TacticsEngine.Board;


namespace WebEditor.Components.DragAndDrop;

public sealed record FigureDragData(Figure Figure, Position Offset) : IDragData
{
    public void HandleDragEnd()
    {
        DragAndDropManager.Shared.DraggedFigure = null;
    }

    public void HandleDragStart() { }

    public void HandleDragEnterTile(BoardData board, Position position)
    {
        DragAndDropManager.Shared.DraggedFigure = new Positioned<Figure>(Figure, position + Offset);
    }

    public void HandleDropTile(BoardData board, Position position)
    {
        board.TryAddFigure(new Positioned<Figure>(Figure, position + Offset));
    }

    public bool CanDrop(BoardData board, Position position) => board.CanAddFigure(position + Offset, Figure);
}