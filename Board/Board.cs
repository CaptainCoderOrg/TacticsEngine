using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace CaptainCoder.TacticsEngine.Board;

public sealed class Board : IEquatable<Board>
{
    public Tile this[int x, int y] => this[new Position(x, y)];
    public Tile this[Position ix] => this.GetTile(ix);
    public HashSet<Position> Tiles { get; set; } = [];
    public PositionMap<Figure> Figures { get; set; } = new();
    public bool Equals(Board? other)
    {
        return other is not null &&
        Tiles.SetEquals(other.Tiles) &&
        Figures.Equals(other.Figures);
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
    public static Tile GetTile(this Board board, Position position)
    {
        if (board.TryGetTile(position, out Tile? tile))
        {
            return tile;
        }
        throw new IndexOutOfRangeException($"No tile at position {position}.");
    }

    public static bool TryGetTile(this Board board, Position position, [NotNullWhen(true)] out Tile? tile)
    {
        tile = null;
        if (!board.Tiles.Contains(position)) { return false; }
        Figure? figure = board.Figures
            .Where(f => new BoundingBox(f.Position, f.Element.Width, f.Element.Height).Positions().Contains(position))
            .Select(f => f.Element)
            .FirstOrDefault();
        tile = new() { Figure = figure };
        return true;
    }
    public static bool TryGetTile(this Board board, int x, int y, [NotNullWhen(true)] out Tile? tile) => board.TryGetTile(new Position(x, y), out tile);

    public static bool CanAddFigure(this Board board, Position position, Figure toAdd)
    {
        BoundingBox bbox = new(position, toAdd.Width, toAdd.Height);
        if (!board.HasTiles(bbox)) { return false; }
        return board.Figures.CanAdd(position, toAdd);
    }
    public static bool CanAddFigure(this Board board, int x, int y, Figure toAdd) => board.CanAddFigure(new Position(x, y), toAdd);

    public static bool TryAddFigure(this Board board, Position position, Figure toAdd)
    {
        BoundingBox bbox = new(position, toAdd.Width, toAdd.Height);
        if (!board.HasTiles(bbox)) { return false; }
        return board.Figures.TryAdd(position, toAdd);
    }

    public static bool TryAddFigure(this Board board, int x, int y, Figure toAdd) => board.TryAddFigure(new Position(x, y), toAdd);

    public static bool HasTiles(this Board board, BoundingBox box) => box.Positions().All(board.Tiles.Contains);

    public static void RemoveTile(this Board board, int x, int y) => board.RemoveTile(new Position(x, y));
    public static void RemoveTile(this Board board, Position position)
    {
        if (board.Tiles.Remove(position))
        {
            board.RemoveFigure(position);
        }
    }

    public static bool RemoveFigure(this Board board, Position position) => board.Figures.TryRemove(position, out _);
    private static JsonSerializerOptions Options { get; } = new()
    {
        Converters = { FigureMapConverter.Shared }
    };
    public static string ToJson(this Board board) => JsonSerializer.Serialize(board, Options);

    public static bool TryFromJson(string json, [NotNullWhen(true)] out Board? board)
    {
        board = JsonSerializer.Deserialize<Board>(json, Options);
        return board is not null;
    }
}