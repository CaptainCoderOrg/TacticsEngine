using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace CaptainCoder.TacticsEngine.Board;

public sealed class BoardData : IEquatable<BoardData>
{
    public Tile this[int x, int y] => this[new Position(x, y)];
    public Tile this[Position ix] => this.GetTile(ix);
    public HashSet<Position> Tiles { get; set; } = [];
    public PositionMap<Figure> Figures { get; set; } = new();
    public bool Equals(BoardData? other)
    {
        return other is not null &&
        Tiles.SetEquals(other.Tiles) &&
        Figures.Equals(other.Figures);
    }
}

public static class BoardExtensions
{
    public static void CreateEmptyTile(this BoardData board, int x, int y) => board.Tiles.Add(new Position(x, y));
    public static void CreateEmptyTiles(this BoardData board, IEnumerable<Position> positions)
    {
        foreach (Position p in positions)
        {
            board.CreateEmptyTile(p.X, p.Y);
        }
    }
    public static bool HasTile(this BoardData board, int x, int y) => board.HasTile(new Position(x, y));
    public static bool HasTile(this BoardData board, Position position) => board.Tiles.Contains(position);
    public static Tile GetTile(this BoardData board, Position position)
    {
        if (board.TryGetTile(position, out Tile? tile))
        {
            return tile;
        }
        throw new IndexOutOfRangeException($"No tile at position {position}.");
    }

    public static bool TryGetTile(this BoardData board, Position position, [NotNullWhen(true)] out Tile? tile)
    {
        tile = null;
        if (!board.Tiles.Contains(position)) { return false; }
        Positioned<Figure>? figure = board.Figures
            .FirstOrDefault(f => f.BoundingBox().Contains(position));
        tile = new() { Figure = figure };
        return true;
    }
    public static bool TryGetTile(this BoardData board, int x, int y, [NotNullWhen(true)] out Tile? tile) => board.TryGetTile(new Position(x, y), out tile);

    public static bool CanAddFigure(this BoardData board, Position position, Figure toAdd)
    {
        BoundingBox bbox = new(position, toAdd.Width, toAdd.Height);
        if (!board.HasTiles(bbox)) { return false; }
        return board.Figures.CanAdd(position, toAdd);
    }
    public static bool CanAddFigure(this BoardData board, int x, int y, Figure toAdd) => board.CanAddFigure(new Position(x, y), toAdd);
    public static bool TryAddFigure(this BoardData board, Positioned<Figure> toAdd)
    {
        if (!board.HasTiles(toAdd.BoundingBox())) { return false; }
        return board.Figures.TryAdd(toAdd);
    }

    public static bool TryAddFigure(this BoardData board, Position position, Figure toAdd)
    {
        BoundingBox bbox = new(position, toAdd.Width, toAdd.Height);
        if (!board.HasTiles(bbox)) { return false; }
        return board.Figures.TryAdd(position, toAdd);
    }

    public static bool TryAddFigure(this BoardData board, int x, int y, Figure toAdd) => board.TryAddFigure(new Position(x, y), toAdd);

    public static bool HasTiles(this BoardData board, BoundingBox box) => box.Positions().All(board.Tiles.Contains);

    public static void RemoveTiles(this BoardData board, IEnumerable<Position> positions)
    {
        foreach (Position position in positions)
        {
            board.RemoveTile(position);
        }
    }

    public static void RemoveTile(this BoardData board, int x, int y) => board.RemoveTile(new Position(x, y));
    public static void RemoveTile(this BoardData board, Position position)
    {
        if (board.Tiles.Remove(position))
        {
            board.TryRemoveFigure(position, out Positioned<Figure>? _);
        }
    }
    public static bool TryRemoveFigure(this BoardData board, Position position, [NotNullWhen(true)] out Positioned<Figure>? removed) =>
        board.Figures.TryRemove(position, out removed);
    public static bool CanMoveFigure(this BoardData board, Position start, Position end)
    {
        if (board.TryRemoveFigure(start, out Positioned<Figure>? removed))
        {
            bool canMove = board.CanAddFigure(end, removed.Element);
            _ = board.TryAddFigure(removed);
            return canMove;
        }
        return false;
    }
    public static bool TryMoveFigure(this BoardData board, Position start, Position end)
    {
        if (board.CanMoveFigure(start, end))
        {
            _ = board.TryRemoveFigure(start, out Positioned<Figure>? removed);
            _ = board.TryAddFigure(removed! with { Position = end });
            return true;
        }
        return false;

    }

    public static BoundingBox BoundingBox(this BoardData board)
    {
        (int rows, int columns) = board.Tiles.Aggregate(new Position(0, 0), Max);
        return new BoundingBox(0, 0, rows, columns);
        static Position Max(Position a, Position b) => a with { X = Math.Max(a.X, b.X), Y = Math.Max(a.Y, b.Y) };
    }

    public static IEnumerable<Positioned<Tile>> TilesData(this BoardData board) =>
        board.Tiles.Select(pos => new Positioned<Tile>(board[pos], pos));

    private static JsonSerializerOptions Options { get; } = new()
    {
        Converters = { FigureMapConverter.Shared }
    };
    public static string ToJson(this BoardData board) => JsonSerializer.Serialize(board, Options);

    public static bool TryFromJson(string json, [NotNullWhen(true)] out BoardData? board)
    {
        board = JsonSerializer.Deserialize<BoardData>(json, Options);
        return board is not null;
    }
}