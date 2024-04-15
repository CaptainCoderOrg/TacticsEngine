
namespace CaptainCoder.TacticsEngine.Board;

public class Board
{
    private readonly Dictionary<Position, Tile> _positions = [];
    public void AddTile(int x, int y) => _positions.Add(new Position(x, y), Tile.Empty);
    public bool HasTile(int x, int y) => _positions.ContainsKey(new Position(x, y));
    public Tile GetTile(int x, int y) => _positions.GetValueOrDefault(new Position(x, y), Tile.None);
}