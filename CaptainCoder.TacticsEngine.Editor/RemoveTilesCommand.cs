using CaptainCoder.TacticsEngine.Board;

namespace CaptainCoder.TacticsEngine.Editor;

public record RemoveTilesCommand(BoardData Board, BoundingBox Selection) : IBoardEditCommand
{
    private readonly BoardData _originalBoard = Board.Copy();
    public BoardData Do()
    {
        BoardData newBoard = _originalBoard.Copy();
        newBoard.RemoveTiles(Selection.Positions());
        return newBoard;
    }

    public BoardData Undo() => _originalBoard;
}