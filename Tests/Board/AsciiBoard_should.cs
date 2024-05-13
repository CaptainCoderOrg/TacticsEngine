namespace CaptainCoder.TacticsEngine.Board.Tests;

using CaptainCoder.Tactics.Board;

using Shouldly;

public class AsciiBoard_should
{

    [Fact]
    public void produce_ascii_board()
    {
        BoardData undertest = new()
        {
            Tiles = [.. new BoundingBox(1, 1, 5, 5).Positions(),
                .. new BoundingBox(7, 2, 3, 2).Positions()],
            Figures = [
                new Positioned<Figure>(new Figure(2, 2), new Position(2, 2)),
                new Positioned<Figure>(new Figure(1, 1), new Position(5, 5)),
                new Positioned<Figure>(new Figure(1, 2), new Position(8, 2)),
            ],
        };

        string actual = undertest.ToAscii();

        string expectedAscii = """
            ..........
            .#####....
            .#AA##.#B#
            .#AA##.#B#
            .#####....
            .####C....
            """.TrimEnd();

        actual.ShouldBe(expectedAscii);
    }
}