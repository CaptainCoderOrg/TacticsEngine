
using CaptainCoder.TacticsEngine.Board;

#pragma warning disable IDE0005 // This import is required but dotnet format removes it
using WebEditor.Components.Board.Clipboard;
#pragma warning restore IDE0005 // This import is required but dotnet format removes it

namespace WebEditor.Components.Board;

public sealed record SelectTilesTool(Position Start, BoardRenderer Target, ClipboardRenderer Clipboard) : BoundingBoxTool(Start, Target)
{
    public override string ColorClass => "select-tiles";

    public override void OnClick(Position position)
    {
        Clipboard.AddSelection(new BoundingBox(position, 1, 1));
    }

    public override void OnDrop(Position position)
    {
        Clipboard.AddSelection(Selection);
    }
}