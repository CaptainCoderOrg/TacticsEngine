using CaptainCoder.TacticsEngine.Board;

namespace CaptainCoder.TacticsEngine.Editor;

public record MoveSelectionCommand(BoardData Board, Position TopLeft, BoardData Selection, IEnumerable<Position> Removing, Action OnUndo) : IBoardEditCommand
{
    private readonly BoardData _originalBoard = Board.Copy();
    public BoardData Do()
    {
        BoardData newBoard = _originalBoard.Copy();
        newBoard.RemoveTiles(Removing);
        newBoard.AddAll(Selection, TopLeft);
        return newBoard;
    }

    public BoardData Undo()
    {
        OnUndo.Invoke();
        return _originalBoard;
    }
}