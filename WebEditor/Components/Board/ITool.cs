using CaptainCoder.TacticsEngine.Board;

namespace WebEditor.Components.Board;

public interface ITool
{
    public Type ComponentType { get; }
    public Dictionary<string, object> ComponentParameters { get; }
    public void OnClick(Position position);
    public void OnDragOver(Position position);
    public void OnDrop(Position position);
}