using CaptainCoder.TacticsEngine.Board;

namespace WebEditor.Components.BoardEditor;

public sealed class TileTool : Tool
{
    public static TileTool Shared { get; } = new();
    public override void OnClick(Board board, Position position)
    {
        if (board.HasTile(position.X, position.Y)) { return; }
        board.CreateEmptyTile(position.X, position.Y);
    }
}