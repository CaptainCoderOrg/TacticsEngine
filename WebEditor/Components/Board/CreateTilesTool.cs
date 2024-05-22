
using CaptainCoder.TacticsEngine.Board;
using CaptainCoder.TacticsEngine.Editor;

namespace WebEditor.Components.Board;

public sealed record class CreateTilesTool(Position Start, BoardRenderer Target, BoardEditor Editor) : BoundingBoxTool(Start, Target)
{
    public override string ColorClass => "add-tiles";

    public override void OnClick(Position position)
    {
        CreateTilesCommand command = new(Editor.Board, new BoundingBox(position, 1, 1));
        Editor.Apply(command);
    }

    public override void OnDrop(Position position)
    {
        CreateTilesCommand command = new(Editor.Board, Selection);
        Editor.Apply(command);
    }
}