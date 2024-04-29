using CaptainCoder.TacticsEngine.Board;

namespace WebEditor.Components.BoardEditor;
public sealed class FigureTool : Tool
{
    public static FigureTool Shared { get; } = new();
    private Positioned<Figure>? _selected;
    public Positioned<Figure>? DraggedFigure { get; private set; }
    private Position _offset = new();

    public override void OnSelectFigure(Board board, Positioned<Figure> figure)
    {
        _selected = figure;
    }

    public override void OnStartDragFigure(Board board, Positioned<Figure> figure, Position offset)
    {
        _offset = offset;
        _selected = figure;
        board.RemoveFigure(figure.Position);
    }

    public override void OnMouseOver(Board board, Position position)
    {
        base.OnMouseOver(board, position);
        if (_selected is Positioned<Figure> figure)
        {
            DraggedFigure = new(figure.Element, position + _offset);
        }
    }

    public override void OnMouseUp(Board board, Position endPosition)
    {
        if (_selected != null && !board.TryAddFigure(endPosition + _offset, _selected.Element))
        {
            board.Figures.Add(_selected);
        }
        _selected = null;
        DraggedFigure = null;
    }
}