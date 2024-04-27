using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace CaptainCoder.TacticsEngine.Board;

public sealed class Board : IEquatable<Board>
{
    public HashSet<Position> TileSet { get; set; } = [];
    public HashSet<Positioned<Figure>> Figures { get; set; } = [];

    public void CreateEmptyTile(int x, int y) => TileSet.Add(new Position(x, y));
    public void CreateEmptyTiles(IEnumerable<Position> positions)
    {
        foreach (Position p in positions)
        {
            CreateEmptyTile(p.X, p.Y);
        }
    }
    public bool HasTile(int x, int y) => TileSet.Contains(new Position(x, y));
    public TileInfo GetTile(int x, int y)
    {
        if (!TileSet.Contains(new Position(x, y))) { return TileInfo.None; }
        FigureInfo? info = Figures
            .Where(f => new BoundingBox(f.Position, f.Element.Width, f.Element.Height).Positions().Contains(new Position(x, y)))
            .Select(f => f.Element)
            .FirstOrDefault();
        Tile tile = new() { Figure = info ?? FigureInfo.None };
        return tile;
    }

    public void AddFigure(int x, int y, Figure toAdd)
    {
        Position position = new(x, y);
        BoundingBox bbox = new(position, toAdd.Width, toAdd.Height);
        if (!bbox.Positions().All(TileSet.Contains)) { throw new ArgumentOutOfRangeException($"Board does not contain a tile at position {x}, {y}"); }
        if (bbox.Positions().Any(IsOccupied)) { throw new InvalidOperationException($"Cannot place figure at position {x}, {y} because it overlaps with another figure."); }
        Figures.Add(new Positioned<Figure>(toAdd, position));
        bool IsOccupied(Position pos) => Figures.Any(f => new BoundingBox(f.Position, f.Element.Width, f.Element.Height).Positions().Contains(pos));
    }

    public void RemoveTile(int x, int y)
    {
        TileSet.Remove(new Position(x, y));
        //_tiles.Remove(new Position(x, y));  
    }

    public bool Equals(Board? other)
    {
        return other is not null &&
        TileSet.SetEquals(other.TileSet) &&
        Figures.SetEquals(other.Figures);
    }
}

public static class BoardExtensions
{
    public static string ToJson(this Board board) => JsonSerializer.Serialize(board);

    public static bool TryFromJson(string json, [NotNullWhen(true)] out Board? board)
    {
        board = JsonSerializer.Deserialize<Board>(json);
        return board is not null;
    }
}