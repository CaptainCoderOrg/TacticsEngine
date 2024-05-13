namespace CaptainCoder.TacticsEngine.Board.Tests;

using CaptainCoder.Tactics.Board;

using Shouldly;

public class BoardEditor_should
{

    /*
     * .####..
     * .####..
     * .#11#..
     * .#11#..
     * .####..
     */
    [Fact]
    public void select_section_of_board_with_tiles()
    {
        BoardData undertest = new()
        {
            Tiles = [.. new BoundingBox(1, 1, 5, 5).Positions()],
        };

        BoardData actualSelected = undertest.SelectSection(new BoundingBox(3, 3, 3, 3));

        BoardData expectedBoard = new()
        {
            Tiles = [.. new BoundingBox(0, 0, 3, 3).Positions()]
        };

        actualSelected.ShouldBe(expectedBoard);
    }
}