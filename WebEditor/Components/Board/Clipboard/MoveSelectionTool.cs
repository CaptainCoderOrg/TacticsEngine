using CaptainCoder.TacticsEngine.Board;

namespace WebEditor.Components.Board.Clipboard;

public sealed class MoveSelectionTool(Position mousePosition) : ITool
{
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

    public required BoardData Selection { get; init; }
    public required BoardRenderer Parent { get; init; }
    public required Position SelectionOffset { get; init; }
    public required Position MouseOffset { get; init; }
    public required Position Origin { get; init; }
    public Position MousePosition => _mousePosition - MouseOffset;
    private Position _mousePosition = mousePosition;

    public void OnClick(Position position) { }

    public void OnDragOver(Position position)
    {
        _mousePosition = position;
    }

    public void OnDrop(Position position)
    {
        var removing = Selection.Tiles.Select(pos => pos + Origin - SelectionOffset);
        Parent.Board.RemoveTiles(removing);
        Parent.Board.AddAll(Selection, MousePosition - SelectionOffset);
        Parent.Redraw();
    }
}