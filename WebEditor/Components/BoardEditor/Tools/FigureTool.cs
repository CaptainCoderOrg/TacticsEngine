using CaptainCoder.TacticsEngine.Board;

namespace WebEditor.Components.BoardEditor;
public sealed class FigureTool : Tool
{
    public static FigureTool Shared { get; } = new();
    public Figure ToDraw { get; private set; } = new() { Width = 1, Height = 2 };
    public Positioned<Figure>? Selected { get; private set; }
    public Positioned<Figure>? Target { get; private set; }
    private Position _offset = new();
    public bool IsDragging { get; private set; } = false;

    public override void OnSelectFigure(Board board, Positioned<Figure> figure)
    {
        Selected = figure;
    }

    public override void OnStartDragFigure(Board board, Positioned<Figure> figure, Position offset)
    {
        _offset = offset;
        IsDragging = true;
        Selected = figure;
        board.RemoveFigure(figure.Position);
    }

    public override void OnMouseOver(Board board, Position position)
    {
        base.OnMouseOver(board, position);
        if (IsDragging && Selected is Positioned<Figure> figure)
        {
            Position withOffset = new(position.X + _offset.X, position.Y + _offset.Y);
            Target = new(figure.Element, withOffset);
        }
    }

    public override void OnMouseUp(Board board, Position endPosition)
    {
        Position withOffset = new(endPosition.X + _offset.X, endPosition.Y + _offset.Y);
        if (Selected != null && !board.TryAddFigure(withOffset, Selected.Element))
        {
            board.Figures.Add(Selected);
        }
        Selected = null;
        Target = null;
        IsDragging = false;
    }
}