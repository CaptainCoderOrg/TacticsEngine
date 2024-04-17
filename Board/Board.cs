using System.Collections.ObjectModel;

namespace CaptainCoder.TacticsEngine.Board;

public sealed class Board
{
    private readonly Dictionary<Position, TileInfo> _tiles = [];
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
}