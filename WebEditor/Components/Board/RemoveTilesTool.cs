using CaptainCoder.TacticsEngine.Board;

namespace WebEditor.Components.Board;

public sealed record class RemoveTilesTool(Position Start, BoardRenderer Target) : BoundingBoxTool(Start, Target)
{
    public override string ColorClass => "remove-tiles";

    public override void OnClick(Position position)
    {
        Target.Board.RemoveTile(position.X, position.Y);
        Target.Redraw();
    }

    public override void OnDrop(Position position)
    {
        Target.Board.RemoveSelection(Selection);
        Target.Redraw();
    }
}