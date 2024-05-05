using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

using Optional;
using Optional.Collections;
using Optional.Linq;

namespace CaptainCoder.TacticsEngine.Board;

public sealed class Board : IEquatable<Board>
{
    public HashSet<Position> Tiles { get; set; } = [];
    public PositionMap<Figure> Figures { get; set; } = [];
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
    public static Option<Tile> GetTile(this Board board, Position position) =>
        board.NoneWhen(board => !board.Tiles.Contains(position))
             .Select(board => board.Figures.FirstOrNone(f => f.BoundingBox().Contains(position)))
             .Select(positionedFigure => new Tile() { Figure = positionedFigure.Select(f => f.Element) });

    public static Option<Tile> GetTile(this Board board, int x, int y) => board.GetTile(new Position(x, y));

    public static bool CanAddFigure(this Board board, Position position, Figure toAdd)
    {
        BoundingBox bbox = new(position, toAdd.Width, toAdd.Height);
        return board.HasTiles(bbox) && board.Figures.CanAdd(position, toAdd);
    }
    public static bool CanAddFigure(this Board board, int x, int y, Figure toAdd) => board.CanAddFigure(new Position(x, y), toAdd);

    public static bool TryAddFigure(this Board board, Position position, Figure toAdd)
    {
        BoundingBox bbox = new(position, toAdd.Width, toAdd.Height);
        return board.HasTiles(bbox) && board.Figures.TryAdd(position, toAdd);
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

    public static bool RemoveFigure(this Board board, Position position) => board.Figures.Remove(position).HasValue;
    public static bool MoveFigure(this Board board, int startX, int startY, int endX, int endY) => board.MoveFigure(new Position(startX, startY), new Position(endX, endY));
    public static bool MoveFigure(this Board board, Position start, Position end)
    {
        foreach (Positioned<Figure> toMove in board.Figures.Remove(start))
        {
            BoundingBox endBox = new(end.X, end.Y, toMove.Element.Width, toMove.Element.Height);
            if (endBox.Positions().Any(board.Figures.IsOccupied))
            {
                board.Figures.Add(toMove);
                return false;
            }
            return board.TryAddFigure(end.X, end.Y, toMove.Element);
        }
        return false;
    }
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