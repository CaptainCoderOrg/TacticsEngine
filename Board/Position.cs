namespace CaptainCoder.TacticsEngine;
public record struct Position(int X, int Y);

public static class PositionExtensions
{
    public static IEnumerable<Position> PositionRect(this Position topLeft, int width, int height)
    {
        for (int boardY = 0; boardY < height; boardY++)
        {
            for (int boardX = 0; boardX < width; boardX++)
            {
                yield return new Position(boardX + topLeft.X, boardY + topLeft.Y);
            }
        }
    }
}