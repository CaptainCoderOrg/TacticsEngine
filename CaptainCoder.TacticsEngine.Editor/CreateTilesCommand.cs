using CaptainCoder.TacticsEngine.Board;

namespace CaptainCoder.TacticsEngine.Editor;

public record CreateTilesCommand(BoardData Board, BoundingBox Selection) : IBoardEditCommand
{
    private readonly BoardData _originalBoard = Board.Copy();
    public BoardData Do()
    {
        Board.CreateEmptyTiles(Selection.Positions());
        return Board;
    }

    public BoardData Undo() => _originalBoard;
}