using CaptainCoder.TacticsEngine.Board;

namespace CaptainCoder.TacticsEngine.Editor;

public interface IBoardEditCommand
{
    public BoardData Do();
    public BoardData Undo();
}