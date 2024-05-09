using CaptainCoder.TacticsEngine.Board;

namespace WebEditor.Tools;

public sealed class TileTool : Tool
{
    public static TileTool Shared { get; } = new();
    public override EventResult OnClick(BoardData board, Position position)
    {
        if (base.OnClick(board, position) is EventResult.Handled) { return EventResult.Handled; };
        if (board.HasTile(position.X, position.Y)) { return EventResult.Unhandled; }
        board.CreateEmptyTile(position.X, position.Y);
        return EventResult.Handled;
    }
}