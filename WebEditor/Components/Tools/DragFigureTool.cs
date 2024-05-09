using CaptainCoder.TacticsEngine.Board;

namespace WebEditor.Tools;
public sealed class DragFigureTool : Tool
{
    public static DragFigureTool Shared { get; } = new();
    private Figure? _selected;
    private Positioned<Figure>? _removed;
    private Positioned<Figure>? _draggedFigure;
    public Positioned<Figure>? DraggedFigure
    {
        get => _draggedFigure;
        private set
        {
            if (_draggedFigure == value) { return; }
            _draggedFigure = value;
            OnDraggedFigureChanged?.Invoke(_draggedFigure);
        }
    }
    private Position _offset;

    public Action<Positioned<Figure>?>? OnDraggedFigureChanged;

    public void ClearSelected()
    {
        _selected = null;
        DraggedFigure = null;
    }

    public override void OnStartDrag(BoardData board, Position position)
    {
        if (board.TryGetTile(position, out Tile? tile) && tile.Figure is Positioned<Figure> figure)
        {
            StartDragFigure(board, figure.Element, position);
        }
    }

    public void StartDragFigure(BoardData board, Figure figure, Position? originalPosition = null)
    {
        ToolManager.Shared.Tool = this;
        _selected = figure;
        _offset = default;
        _removed = null;
        if (originalPosition.HasValue && board.TryRemoveFigure(originalPosition.Value, out _removed))
        {
            _offset = _removed.Position - originalPosition.Value;
        }
    }

    public override void OnMouseOver(BoardData board, Position position)
    {
        base.OnMouseOver(board, position);
        if (_selected is not null)
        {
            DraggedFigure = new(_selected, position + _offset);
        }
    }

    public override void OnMouseUp(BoardData board, Position endPosition)
    {
        if (_selected is not null && board.TryAddFigure(endPosition + _offset, _selected))
        {
            FigureTool.Shared.Selected = new Positioned<Figure>(_selected, endPosition + _offset);
        }
        else if (_removed is not null && board.TryAddFigure(_removed))
        {
            FigureTool.Shared.Selected = _removed;
        }
        ToolManager.Shared.Tool = FigureTool.Shared;
        ClearSelected();
    }
}