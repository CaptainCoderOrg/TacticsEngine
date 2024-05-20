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

    public static string ToDimensionString(this BoundingBox box) => box switch
    {
        BoundingBox(_, 1, int height) => height.ToString(),
        BoundingBox(_, int width, 1) => width.ToString(),
        BoundingBox(_, int width, int height) => $"{width} x {height}",
    };

    public static BoundingBox CreateBoundingBox(this Position first, Position other)
    {
        int left = Math.Min(first.X, other.X);
        int right = Math.Max(first.X, other.X);
        int top = Math.Min(first.Y, other.Y);
        int bottom = Math.Max(first.Y, other.Y);
        return new BoundingBox(left, top, right - left + 1, bottom - top + 1);
    }

    public static bool Contains(this BoundingBox box, Position position) =>
        position.X >= box.TopLeft.X &&
        position.X < box.TopLeft.X + box.Width &&
        position.Y >= box.TopLeft.Y &&
        position.Y < box.TopLeft.Y + box.Height;

    public static int Left(this BoundingBox box) => box.TopLeft.X;

    public static int Right(this BoundingBox box) => box.TopLeft.X + box.Width - 1;

    public static int Top(this BoundingBox box) => box.TopLeft.Y;

    public static int Bottom(this BoundingBox box) => box.TopLeft.Y + box.Height - 1;

    public static bool Contains(this BoundingBox box, BoundingBox toCheck) =>
        toCheck.Left() >= box.Left() &&
        toCheck.Right() <= box.Right() &&
        toCheck.Top() >= box.Top() &&
        toCheck.Bottom() <= box.Bottom();

    /// <summary>
    /// Returns a <see cref="BoundingBox"/> that contains both of the specified <see cref="BoundingBox"/>.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static BoundingBox Fit(this BoundingBox a, BoundingBox b)
    {
        int left = Math.Min(a.TopLeft.X, b.TopLeft.X);
        int top = Math.Min(a.TopLeft.Y, b.TopLeft.Y);
        int right = Math.Max(a.Right(), b.Right());
        int bottom = Math.Max(a.Bottom(), b.Bottom());
        return new()
        {
            TopLeft = new Position(left, top),
            Width = right - left + 1,
            Height = bottom - top + 1,
        };
    }
}