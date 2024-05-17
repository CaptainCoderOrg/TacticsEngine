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
            Tile tile = board[position];
            if (tile.Figure is null) { tiles.Add(position); }
            if (tile.Figure is Positioned<Figure> figure && selection.Contains(figure.BoundingBox()))
            {
                AddFigure(figure);
            }
        }

        figures = [.. figures.Where(f => f.BoundingBox().Positions().All(p => selection.Contains(p)))];
        tiles.UnionWith(figures.SelectMany(f => f.BoundingBox().Positions()));

        return new BoardData()
        {
            Tiles = [.. tiles.Select(Normalize)],
            Figures = [.. figures.Select(f => f with { Position = Normalize(f.Position) })]
        };

        Position Normalize(Position position) => position - selection.TopLeft;
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