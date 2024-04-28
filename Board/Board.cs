using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace CaptainCoder.TacticsEngine.Board;

public sealed class Board : IEquatable<Board>
{
    public HashSet<Position> Tiles { get; set; } = [];
    public HashSet<Positioned<Figure>> Figures { get; set; } = [];
    public bool Equals(Board? other)
    {
        return other is not null &&
        Tiles.SetEquals(other.Tiles) &&
        Figures.SetEquals(other.Figures);
    }
}

public static class BoardExtensions
{
    public static void CreateEmptyTile(this Board board, int x, int y) => board.Tiles.Add(new Position(x, y));
    public static void CreateEmptyTiles(this Board board, IEnumerable<Position> positions)
    {
        foreach (Position p in positions)
        {
            board.CreateEmptyTile(p.X, p.Y);
        }
    }
    public static bool HasTile(this Board board, int x, int y) => board.Tiles.Contains(new Position(x, y));
    public static TileInfo GetTile(this Board board, Position position)
    {
        if (!board.Tiles.Contains(position)) { return TileInfo.None; }
        FigureInfo? info = board.Figures
            .Where(f => new BoundingBox(f.Position, f.Element.Width, f.Element.Height).Positions().Contains(position))
            .Select(f => f.Element)
            .FirstOrDefault();
        Tile tile = new() { Figure = info ?? FigureInfo.None };
        return tile;
    }
    public static TileInfo GetTile(this Board board, int x, int y) => board.GetTile(new Position(x, y));

    public static void AddFigure(this Board board, int x, int y, Figure toAdd)
    {
        Position position = new(x, y);
        BoundingBox bbox = new(position, toAdd.Width, toAdd.Height);
        if (!bbox.Positions().All(board.Tiles.Contains)) { throw new ArgumentOutOfRangeException($"Board does not contain a tile at position {x}, {y}"); }
        if (bbox.Positions().Any(board.IsOccupied)) { throw new InvalidOperationException($"Cannot place figure at position {x}, {y} because it overlaps with another figure."); }
        board.Figures.Add(new Positioned<Figure>(toAdd, position));
    }

    public static void RemoveTile(this Board board, int x, int y) => board.RemoveTile(new Position(x, y));
    public static void RemoveTile(this Board board, Position position)
    {
        if (board.Tiles.Remove(position))
        {
            board.RemoveFigure(position);
        }
    }

    public static bool RemoveFigure(this Board board, Position position)
    {
        Positioned<Figure>? figure = board.Figures
            .Where(f => f.BoundingBox().Contains(position))
            .Select(f => f)
            .FirstOrDefault();
        if (figure is not null) { return board.Figures.Remove(figure); }
        return false;
    }
    public static bool MoveFigure(this Board board, int startX, int startY, int endX, int endY) => board.MoveFigure(new Position(startX, startY), new Position(endX, endY));
    public static bool MoveFigure(this Board board, Position start, Position end)
    {
        Positioned<Figure>? toMove = board.Figures.FirstOrDefault(f => f.Position == start);
        if (toMove is null) { return false; }
        BoundingBox endBox = new(end.X, end.Y, toMove.Element.Width, toMove.Element.Height);
        IEnumerable<BoundingBox> others = board.Figures.Where(f => f != toMove).Select(f => f.BoundingBox());
        if (endBox.OverlapsAny(others)) { return false; }
        board.AddFigure(end.X, end.Y, toMove.Element);
        board.Figures.Remove(toMove);
        return true;
    }

    public static bool IsOccupied(this Board board, Position pos) => board.Figures.Any(f => new BoundingBox(f.Position, f.Element.Width, f.Element.Height).Positions().Contains(pos));

    public static string ToJson(this Board board) => JsonSerializer.Serialize(board);

    public static bool TryFromJson(string json, [NotNullWhen(true)] out Board? board)
    {
        board = JsonSerializer.Deserialize<Board>(json);
        return board is not null;
    }
}