using CaptainCoder.TacticsEngine.Board;

namespace WebEditor.Components.DragAndDrop;

internal class DragAndDropManager
{
    private Positioned<Figure>? _draggedFigure;
    public static DragAndDropManager Shared { get; } = new DragAndDropManager();
    public IDragData? DraggedData { get; set; }
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