using CaptainCoder.TacticsEngine.Board;

namespace CaptainCoder.Tactics.Board;

public static class BoardEditorExtensions
{
    public static BoardData SelectSection(this BoardData board, BoundingBox selection)
    {
        return new BoardData() { Tiles = [.. new BoundingBox(0, 0, 3, 3).Positions()] };
    }
}