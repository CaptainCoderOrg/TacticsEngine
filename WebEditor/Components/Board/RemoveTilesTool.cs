using CaptainCoder.TacticsEngine.Board;
using CaptainCoder.TacticsEngine.Editor;

namespace WebEditor.Components.Board;

public sealed record class RemoveTilesTool(Position Start, BoardRenderer Target, BoardEditor Editor) : BoundingBoxTool(Start, Target)
{
    public override string ColorClass => "remove-tiles";

    public override void OnClick(Position position)
    {
        Editor.Apply(new RemoveTilesCommand(Editor.Board, new BoundingBox(position, 1, 1)));
    }

    public override void OnDrop(Position position)
    {
        Editor.Apply(new RemoveTilesCommand(Editor.Board, Selection));
    }
}