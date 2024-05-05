using CaptainCoder.TacticsEngine.Board;

using CaptainCoder.Optional;
using Optional;
using Optional.Linq;
using Optional.Unsafe;

namespace WebEditor.Tools;
public sealed class FigureTool : Tool
{
    public static FigureTool Shared { get; } = new();
    private Option<Figure> _selected = Option.None<Figure>();
    private Option<Positioned<Figure>> _removed = Option.None<Positioned<Figure>>();
    public Option<Positioned<Figure>> DraggedFigure { get; private set; }
    private Position _offset = new();

    public override void OnStartDragFigure(Board board, Figure figure, Position? startDragPosition = null)
    {
        Option<Position> start = startDragPosition.HasValue ? startDragPosition!.Value.Some() : Option.None<Position>();
        _selected = figure.Some();
        _removed = start.SelectMany(board.RemoveFigure);
        _offset = _removed.SelectMany(figure => start.Select(start => figure.Position - start)).ValueOrDefault();
    }

    public override void OnMouseOver(Board board, Position position)
    {
        base.OnMouseOver(board, position);
        DraggedFigure = _selected.Select(f => new Positioned<Figure>(f, position + _offset));
    }

    public override void OnMouseUp(Board board, Position endPosition)
    {
        var figureMoved = _selected.SelectMany(figure => board.TryAddFigure(endPosition + _offset, figure));
        figureMoved.MatchNone(() => _removed.ForEach(board.TryAddFigure));
        _selected = Option.None<Figure>();
        DraggedFigure = Option.None<Positioned<Figure>>();
    }
}