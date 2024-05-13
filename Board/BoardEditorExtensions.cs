using CaptainCoder.TacticsEngine.Board;

namespace CaptainCoder.Tactics.Board;

public static class BoardEditorExtensions
{
    public static BoardData SelectSection(this BoardData board, BoundingBox selection)
    {
        HashSet<Position> tiles = [];
        HashSet<Positioned<Figure>> figures = [];

        foreach (Position position in selection.Positions())
        {
            if (!board.HasTile(position)) { continue; }
            tiles.Add(position);
            AddFigure(board[position].Figure);
        }

        Position topLeft = tiles.Aggregate(selection.TopLeft, (a, b) => a with { X = Math.Min(a.X, b.X), Y = Math.Min(a.Y, b.Y) });

        return new BoardData()
        {
            Tiles = [.. tiles.Select(Normalize)],
            Figures = [.. figures.Select(f => f with { Position = Normalize(f.Position) })]
        };

        Position Normalize(Position position) => position - topLeft;
        void AddFigure(Positioned<Figure>? figure)
        {
            if (figure is null) { return; }
            figures.Add(figure);
            tiles.UnionWith(figure.BoundingBox()
                                  .Positions());
        }
    }
}