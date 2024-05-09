using CaptainCoder.TacticsEngine.Board;

namespace WebEditor.Tools;

public sealed class EraseTool : Tool
{
    public static EraseTool Shared { get; } = new();

    public override EventResult OnClick(BoardData board, Position position)
    {
        if (base.OnClick(board, position) is EventResult.Handled) { return EventResult.Handled; };
        if (!board.HasTile(position.X, position.Y)) { return EventResult.Unhandled; }
        board.RemoveTile(position.X, position.Y);
        return EventResult.Handled;
    }
}