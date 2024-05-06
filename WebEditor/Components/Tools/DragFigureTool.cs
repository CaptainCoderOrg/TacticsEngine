using CaptainCoder.TacticsEngine.Board;

namespace WebEditor.Tools;
public sealed class DragFigureTool : Tool
{
    public static DragFigureTool Shared { get; } = new();
    private Figure? _selected;
    private Positioned<Figure>? _removed;
    public Positioned<Figure>? DraggedFigure { get; private set; }
    private Position _offset;

    public override void OnStartDrag(Board board, Position position)
    {
        if (board.TryGetTile(position, out Tile? tile) && tile.Figure is Positioned<Figure> figure)
        {
            StartDragFigure(board, figure.Element, position);
        }
    }

    public void StartDragFigure(Board board, Figure figure, Position? originalPosition = null)
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

    public override void OnMouseOver(Board board, Position position)
    {
        base.OnMouseOver(board, position);
        if (_selected is not null)
        {
            DraggedFigure = new(_selected, position + _offset);
        }
    }

    public override void OnMouseUp(Board board, Position endPosition)
    {
        if (_selected is not null && !board.TryAddFigure(endPosition + _offset, _selected))
        {
            if (_removed is not null)
            {
                board.TryAddFigure(_removed);
            }
        }
        _selected = null;
        DraggedFigure = null;
        ToolManager.Shared.Tool = FigureTool.Shared;
    }
}