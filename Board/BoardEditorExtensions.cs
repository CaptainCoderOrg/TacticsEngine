using System.Diagnostics;

namespace CaptainCoder.TacticsEngine.Board;

public static class BoardEditorExtensions
{
    public static BoardData GetSelection(this BoardData board, BoundingBox selection)
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

    public static void RemoveSelection(this BoardData board, BoundingBox selection)
    {
        foreach (Position position in selection.Positions())
        {
            board.RemoveTile(position);
        }
    }

    public static void AddAll(this BoardData board, BoardData toAdd, Position offset)
    {
        board.CreateEmptyTiles(toAdd.Tiles.Select(Translate));
        IEnumerable<Positioned<Figure>> overlapping = toAdd.Figures.Select(f => f with { Position = Translate(f.Position) })
                                                                   .Where(f => !board.TryAddFigure(f));
        foreach (var figure in overlapping)
        {
            RemoveExistingFigures(figure.BoundingBox().Positions());
            bool wasAdded = board.TryAddFigure(figure);
            Debug.Assert(wasAdded, $"Could not add {figure} to board.");
        }

        Position Translate(Position position) => position + offset;

        void RemoveExistingFigures(IEnumerable<Position> positions)
        {
            foreach (Position pos in positions)
            {
                _ = board.TryRemoveFigure(pos, out _);
            }
        }
    }
}