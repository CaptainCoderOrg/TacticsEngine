
using CaptainCoder.TacticsEngine.Board;

using WebEditor.Components.Board.Clipboard;

namespace WebEditor.Components.Board;

public sealed record SelectTilesTool(Position Start, BoardRenderer Target, ClipboardRenderer Clipboard) : BoundingBoxTool(Start, Target)
{
    public override string ColorClass => "select-tiles";

    public override void OnClick(Position position) { }

    public override void OnDrop(Position position)
    {
        Clipboard.AddSelection(Selection);
    }
}