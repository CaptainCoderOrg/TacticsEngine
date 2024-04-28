using CaptainCoder.TacticsEngine.Board;

namespace WebEditor.Components.BoardEditor;
public sealed class FigureTool : Tool
{
    public static FigureTool Shared { get; } = new();
    public Figure ToDraw { get; private set; } = new() { Width = 1, Height = 2 };
    public Positioned<Figure>? Selected { get; private set; }
    public bool IsDragging { get; private set; } = false;

    public override void OnSelectFigure(Board board, Positioned<Figure> figure)
    {
        Selected = figure;
        Console.WriteLine($"Selected: {Selected}");
    }

    public override void OnStartDragFigure(Board board, Positioned<Figure> figure)
    {
        IsDragging = true;
        Selected = figure;
        board.RemoveFigure(figure.Position);
        Console.WriteLine($"OnStartDrag: {figure}");
    }

    public override void OnMouseOver(Board board, Position position)
    {
        base.OnMouseOver(board, position);
    }

    public override void OnMouseUp(Board board, Position endPosition)
    {
        Console.WriteLine($"OnEndDrag: {Selected} to {endPosition}");
        if (Selected != null)
        {
            if (!board.Figures.TryAdd(endPosition, Selected.Element))
            {
                board.Figures.Add(Selected);
            }
        }
        Selected = null;
    }
}