

namespace CaptainCoder.TacticsEngine.Board;

public class Board
{
    private readonly Dictionary<Position, TileInfo> _tiles = [];
    public void AddTile(int x, int y) => _tiles.Add(new Position(x, y), TileInfo.Empty);
    public bool HasTile(int x, int y) => _tiles.ContainsKey(new Position(x, y));
    public TileInfo GetTile(int x, int y) => _tiles.GetValueOrDefault(new Position(x, y), TileInfo.None);

    public void AddFigure(int x, int y, Figure toAdd)
    {
        if (_tiles[new Position(x, y)].HasFigure()) { throw new InvalidOperationException(); }
        _tiles[new Position(x, y)] = new Tile() { Figure = toAdd };
    }
}