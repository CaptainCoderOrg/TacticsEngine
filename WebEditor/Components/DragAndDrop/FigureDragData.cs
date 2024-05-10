using CaptainCoder.TacticsEngine.Board;
namespace WebEditor.Components.DragAndDrop;

public sealed record FigureDragData(Figure Figure, Position Offset) : IDragData
{
    public void HandleDragStart() { }

    public void HandleDragOverTile(BoardData board, Position position)
    {
        DragAndDropManager.Shared.DraggedFigure = new Positioned<Figure>(Figure, position + Offset);
    }

    public void HandleDropTile(BoardData board, Position position, Action? onSuccess)
    {
        if (board.TryAddFigure(new Positioned<Figure>(Figure, position + Offset)))
        {
            onSuccess?.Invoke();
        }
    }

    public bool CanDrop(BoardData board, Position position) => board.CanAddFigure(position, Figure);

    public void HandleDragEnd()
    {
        DragAndDropManager.Shared.DraggedFigure = null;
    }
}