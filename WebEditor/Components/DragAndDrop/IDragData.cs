using CaptainCoder.TacticsEngine.Board;

namespace WebEditor.Components.DragAndDrop;

public interface IDragData
{
    public void HandleDragStart();
    public void HandleDragOverTile(BoardData board, Position position);
    public bool CanDrop(BoardData board, Position position);
    public void HandleDropTile(BoardData board, Position position, Action? onSuccess = null);
    public void HandleDragEnd();
}