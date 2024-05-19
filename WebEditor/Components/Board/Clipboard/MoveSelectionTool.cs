using CaptainCoder.TacticsEngine.Board;

namespace WebEditor.Components.Board.Clipboard;

public sealed class MoveSelectionTool(Position mousePosition) : ITool
{
    public required BoardData Selection { get; init; }
    public required BoardRenderer Parent { get; init; }
    public required Position SelectionOffset { get; init; }
    public required Position MouseOffset { get; init; }
    private Position _mousePosition = mousePosition;
    public Position MousePosition => _mousePosition - MouseOffset;
    public Type ComponentType { get; } = typeof(MoveSelectionRenderer);

    private Dictionary<string, object>? _parameters;
    public Dictionary<string, object> ComponentParameters
    {
        get
        {
            if (_parameters is null)
            {
                _parameters = new() { { nameof(MoveSelectionRenderer.Tool), this } };
            }
            return _parameters;
        }
    }

    public void OnClick(Position position) { }

    public void OnDragOver(Position position)
    {
        _mousePosition = position;
    }

    public void OnDrop(Position position) { }
}