
using CaptainCoder.TacticsEngine.Board;

namespace WebEditor.Components.Board;

public sealed record class CreateTilesTool(Position Start, BoardRenderer Target) : BoundingBoxTool(Start, Target)
{
    public override string ColorClass => "add-tiles";

    public override void OnClick(Position position)
    {
        Target.Board.CreateEmptyTile(position.X, position.Y);
        Target.Redraw();
    }

    public override void OnDrop(Position position)
    {
        Target.Board.CreateEmptyTiles(Selection.Positions());
        Target.Redraw();
    }
}