using CaptainCoder.TacticsEngine.Board;

namespace WebEditor.Components.DragAndDrop;

public interface IDragData
{
    public void HandleDragStart();
    public void HandleDragEnd();
    public void HandleDragEnterTile(BoardData board, Position position);
    public void HandleDropTile(BoardData board, Position position);
}