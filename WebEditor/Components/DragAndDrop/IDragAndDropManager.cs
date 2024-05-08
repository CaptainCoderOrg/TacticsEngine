using CaptainCoder.TacticsEngine.Board;

namespace WebEditor.Components.DragAndDrop;

public interface IDragAndDropManager
{
    public Figure? DraggedData { get; set; }
}

internal class DragAndDropManager : IDragAndDropManager
{
    private Figure? _draggedData;

    public static IDragAndDropManager Shared { get; } = new DragAndDropManager();
    public Figure? DraggedData
    {
        get => _draggedData;
        set
        {
            _draggedData = value;
        }
    }
}