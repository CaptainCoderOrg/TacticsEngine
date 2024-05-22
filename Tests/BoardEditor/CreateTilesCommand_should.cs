using CaptainCoder.TacticsEngine.Board;

using Shouldly;

using Tests;

namespace CaptainCoder.TacticsEngine.Editor.Tests;

public class CreateTilesCommand_should
{
    [Fact]
    public void add_tiles_on_do()
    {
        // Arrange
        BoardData emptyBoard = new();
        CreateTilesCommand underTest = new(emptyBoard, new BoundingBox(3, 4, 3, 2));

        // Act
        BoardData updatedBoard = underTest.Do();

        // Assert
        updatedBoard.Tiles.Count.ShouldBe(6);
        Position[] expectedTiles = [
            new Position(3, 4),
            new Position(4, 4),
            new Position(5, 4),
            new Position(3, 5),
            new Position(4, 5),
            new Position(5, 5),
        ];
        updatedBoard.Tiles.ShouldBeSubsetOf(expectedTiles);
    }

    [Fact]
    public void remove_tiles_on_undo()
    {
        //Arrange
        BoardData startingBoard = new()
        {
            Tiles = [.. new BoundingBox(1, 2, 3, 2).Positions()],
        };

        BoardData originalState = new()
        {
            Tiles = [.. new BoundingBox(1, 2, 3, 2).Positions()],
        };

        CreateTilesCommand underTest = new(startingBoard, new BoundingBox(3, 2, 2, 3));
        _ = underTest.Do();

        // Act
        BoardData actual = underTest.Undo();
        actual.ShouldBe(originalState, ErrorMessages.BoardCompareError(originalState, actual));
    }

    [Fact]
    public void handle_undo_redo()
    {
        BoardData emptyBoard = new();
        CreateTilesCommand first = new(emptyBoard, new BoundingBox(0, 0, 2, 2));
        BoardData afterFirst = first.Do();
        CreateTilesCommand second = new(afterFirst, new BoundingBox(2, 2, 2, 2));
        BoardData afterSecond = second.Do();
        BoardData afterFirstUndo = second.Undo();
        BoardData afterSecondUndo = first.Undo();
        BoardData firstRedo = first.Do();
        BoardData secondRedo = second.Do();

        afterFirstUndo.ShouldBe(afterFirst);
        afterSecondUndo.ShouldBe(new BoardData());
        firstRedo.ShouldBe(afterFirst);
        secondRedo.ShouldBe(afterSecond);
    }
}