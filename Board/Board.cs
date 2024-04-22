using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

using CaptainCoder.Json;


namespace CaptainCoder.TacticsEngine.Board;

public sealed class Board : IEquatable<Board>
{

#pragma warning disable IDE0044 // Add readonly modifier
    [JsonInclude]
    private Dictionary<Position, TileInfo> _tiles = [];
#pragma warning restore IDE0044 // Add readonly modifier
    [JsonIgnore]
    public IReadOnlyDictionary<Position, TileInfo> Tiles => new ReadOnlyDictionary<Position, TileInfo>(_tiles);

    public void CreateEmptyTile(int x, int y) => _tiles.Add(new Position(x, y), TileInfo.Empty);
    public void CreateEmptyTiles(IEnumerable<Position> positions)
    {
        foreach (Position p in positions)
        {
            CreateEmptyTile(p.X, p.Y);
        }
    }
    public bool HasTile(int x, int y) => _tiles.ContainsKey(new Position(x, y));
    public TileInfo GetTile(int x, int y) => _tiles.GetValueOrDefault(new Position(x, y), TileInfo.None);

    public void AddFigure(int x, int y, Figure toAdd)
    {
        BoundingBox bbox = new(new Position(x, y), toAdd.Width, toAdd.Height);
        foreach (Position position in bbox.Positions())
        {
            if (!_tiles.TryGetValue(position, out TileInfo? tileInfo)) { throw new ArgumentOutOfRangeException($"Board does not contain a tile at position {x}, {y}"); }
            if (tileInfo is Tile tile && tile.Figure is not NoFigure) { throw new InvalidOperationException($"Cannot place figure at position {x}, {y}. Tile at {position} is already occupied by {tile.Figure} "); }
        }
        foreach (Position position in bbox.Positions())
        {
            _tiles[position] = new Tile() { Figure = toAdd };
        }
    }

    public void RemoveTile(int x, int y) => _tiles.Remove(new Position(x, y));

    public bool Equals(Board? other) =>
        other is not null &&
        _tiles.Count == other._tiles.Count &&
        _tiles.All(kvp => other._tiles.TryGetValue(kvp.Key, out TileInfo? otherTile) && kvp.Value.Equals(otherTile));
}

public static class BoardExtensions
{
    private static JsonSerializerOptions Options { get; } = new()
    {
        Converters =
        {
            new DictionaryJsonConverter<Position, TileInfo>(),
        },
    };
    public static string ToJson(this Board board) => JsonSerializer.Serialize(board, Options);

    public static bool TryFromJson(string json, [NotNullWhen(true)] out Board? board)
    {
        board = JsonSerializer.Deserialize<Board>(json, Options);
        return board is not null;
    }
}