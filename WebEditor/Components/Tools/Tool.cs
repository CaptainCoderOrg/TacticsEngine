using CaptainCoder.TacticsEngine.Board;

namespace WebEditor.Tools;

public abstract class Tool
{
    public virtual EventResult OnClick(BoardData board, Position position)
    {
        return EventResult.Unhandled;
    }
    public virtual void OnMouseOver(BoardData board, Position position) { }
    public virtual void OnMouseUp(BoardData board, Position endPosition) { }
}