using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

using CaptainCoder.Dungeoneering.DungeonMap.IO;

using Newtonsoft.Json;

namespace CaptainCoder.TacticsEngine.Board;

public sealed class Board : IEquatable<Board>
{
    [JsonRequired]
    private readonly Dictionary<Position, TileInfo> _tiles = [];
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
            if (!_tiles.TryGetValue(position, out TileInfo tileInfo)) { throw new ArgumentOutOfRangeException($"Board does not contain a tile at position {x}, {y}"); }
            if (tileInfo is Tile tile && tile.Figure is not NoFigure) { throw new InvalidOperationException($"Cannot place figure at position {x}, {y}. Tile at {position} is already occupied by {tile.Figure} "); }
        }
        foreach (Position position in bbox.Positions())
        {
            _tiles[position] = new Tile() { Figure = toAdd };
        }
    }

    public void RemoveTile(int x, int y) => _tiles.Remove(new Position(x, y));

    public bool Equals(Board other) =>
        _tiles.Count == other._tiles.Count &&
        _tiles.All(kvp => other._tiles.TryGetValue(kvp.Key, out TileInfo otherTile) && kvp.Value.Equals(otherTile));
}

public static class BoardExtensions
{
    public static string ToJson(this Board board)
    {
        JsonSerializerSettings settings = new();
        settings.Converters = [new DictionaryJsonConverter<Position, TileInfo>(), FigureInfoConverter.Shared];
        settings.TypeNameHandling = TypeNameHandling.Auto;
        return JsonConvert.SerializeObject(board, settings);
    }

    public static bool TryFromJson(string json, [NotNullWhen(true)] out Board board)
    {
        JsonSerializerSettings settings = new();
        settings.Converters = [new DictionaryJsonConverter<Position, TileInfo>(), FigureInfoConverter.Shared];
        settings.TypeNameHandling = TypeNameHandling.Auto;
        board = JsonConvert.DeserializeObject<Board>(json, settings)!;
        return board is not null;
    }
}