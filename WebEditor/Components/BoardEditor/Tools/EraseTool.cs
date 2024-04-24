using CaptainCoder.TacticsEngine.Board;

namespace WebEditor.Components.BoardEditor;

public sealed class EraseTool : Tool
{
    public static EraseTool Shared { get; } = new();

    public override void OnClick(Board board, Position position)
    {
        if (!board.HasTile(position.X, position.Y)) { return; }
        board.RemoveTile(position.X, position.Y);
    }
}