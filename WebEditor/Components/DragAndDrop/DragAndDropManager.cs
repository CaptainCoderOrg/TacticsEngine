using CaptainCoder.TacticsEngine.Board;

namespace WebEditor.Components.DragAndDrop;

internal sealed class DragAndDropManager
{
    public static DragAndDropManager Shared { get; } = new DragAndDropManager();

    private IDragData? _draggedData;
    public IDragData? DraggedData
    {
        get => _draggedData;
        set
        {
            if (_draggedData == value) { return; }
            _draggedData = value;
            OnDragDataChange?.Invoke(_draggedData);
        }
    }
    public event Action<IDragData?>? OnDragDataChange;

    private Positioned<Figure>? _draggedFigure;
    public Positioned<Figure>? DraggedFigure
    {
        get => _draggedFigure;
        set
        {
            if (_draggedFigure == value) { return; }
            _draggedFigure = value;
            OnDraggedFigureChange?.Invoke(_draggedFigure);
        }
    }
    public event Action<Positioned<Figure>?>? OnDraggedFigureChange;
}