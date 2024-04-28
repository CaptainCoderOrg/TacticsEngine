namespace CaptainCoder.TacticsEngine.Board;
public sealed record Positioned<T>(T Element, Position Position);

public static class PositionedExtensions
{
    public static BoundingBox BoundingBox<T>(this Positioned<T> positioned) where T : IHasSize
    {
        return new BoundingBox(positioned.Position, positioned.Element.Width, positioned.Element.Height);
    }

}