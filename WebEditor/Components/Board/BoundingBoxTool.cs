using CaptainCoder.TacticsEngine.Board;

namespace WebEditor.Components.Board;

public abstract record BoundingBoxTool(Position Start, BoardRenderer Target) : ITool
{
    public BoundingBox Selection { get; private set; } = new(Start, 1, 1);

    public Type ComponentType { get; } = typeof(BoundingBoxRenderer);

    public Dictionary<string, object> ComponentParameters => new()
    {
        { nameof(BoundingBoxRenderer.Rectangle), Selection },
        { nameof(BoundingBoxRenderer.ColorClass), ColorClass },
    };

    public virtual void OnDragOver(Position position)
    {
        Selection = Start.CreateBoundingBox(position);
    }

    public abstract string ColorClass { get; }
    public abstract void OnClick(Position position);
    public abstract void OnDrop(Position position);
}