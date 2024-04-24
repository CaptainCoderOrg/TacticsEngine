using CaptainCoder.TacticsEngine.Board;

namespace WebEditor.Components.BoardEditor;

public class ToolManager
{
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
    public event Action<Board, Position>? OnClick;
    public event Action<Board, Position>? OnMouseOver;
    public void Click(Board board, Position position)
    {
        _tool.OnClick(board, position);
        OnClick?.Invoke(board, position);
    }

    public void MouseOver(Board board, Position position)
    {
        _tool.OnMouseOver(board, position);
        OnMouseOver?.Invoke(board, position);
    }
}

public abstract class Tool
{
    public virtual void OnClick(Board board, Position position) { }
    public virtual void OnMouseOver(Board board, Position position) { }
}