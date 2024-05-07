using CaptainCoder.TacticsEngine.Board;

namespace WebEditor.Tools;

public class ToolManager
{
    internal static ToolManager Shared { get; } = new();
    private Tool _tool = TileTool.Shared;
    public Tool Tool
    {
        get => _tool;
        set
        {
            if (_tool == value) { return; }
            _tool = value;
            OnChange?.Invoke(_tool);
        }
    }
    public event Action<Tool>? OnChange;
    public void SelectTile(Board board, Position position)
    {
        _tool.OnClick(board, position);
    }

    public void MouseOver(Board board, Position position)
    {
        _tool.OnMouseOver(board, position);
    }

    public void StartDrag(Board board, Position position)
    {
        _tool.OnStartDrag(board, position);
    }

    public void MouseUp(Board board, Position position)
    {
        _tool.OnMouseUp(board, position);
    }
}