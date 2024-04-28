namespace CaptainCoder.TacticsEngine.Board;

public record struct BoundingBox(Position TopLeft, int Width, int Height)
{
    public BoundingBox(int left, int top, int width, int height) : this(new Position(left, top), width, height) { }
}

public static class BoundingBoxExtensions
{
    public static IEnumerable<Position> Positions(this BoundingBox box)
    {
        for (int boardY = 0; boardY < box.Height; boardY++)
        {
            for (int boardX = 0; boardX < box.Width; boardX++)
            {
                yield return new Position(boardX + box.TopLeft.X, boardY + box.TopLeft.Y);
            }
        }
    }

    public static bool OverlapsWith(this BoundingBox box, BoundingBox other) => box.Positions().Any(pos => other.Contains(pos));

    public static bool OverlapsAny(this BoundingBox box, IEnumerable<BoundingBox> others) => others.Any(other => box.OverlapsWith(other));
    public static bool Contains(this BoundingBox box, Position toCheck) => box.Positions().Any(toCheck.Equals);
}