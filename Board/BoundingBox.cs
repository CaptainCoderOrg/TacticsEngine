namespace CaptainCoder.TacticsEngine.Board;

public record struct BoundingBox(Position TopLeft, int Width, int Height);

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
}