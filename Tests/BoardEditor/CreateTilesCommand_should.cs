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
}