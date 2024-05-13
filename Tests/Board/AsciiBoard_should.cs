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

    [Fact]
    public void load_from_string()
    {
        /* 0123456789
         * .......... 0
         * .#####.... 1
         * .##AA#..BB 2
         * .#####..BB 3
         * .CC###..BB 4
         * .CC####DDD 5
         * ......#DDD 6
         * ......#DDD 7
         */

        string toParse = """
            ..........
            .#####....
            .##AA#..BB
            .#####..BB
            .CC###..BB
            .CC####DDD
            ......#DDD
            ......#DDD
            ......####
            """;

        BoardData actual = toParse.ToBoardData();

        BoardData expectedBoard = new()
        {
            Tiles = [
                .. new BoundingBox(1, 1, 5, 5).Positions(),
                .. new BoundingBox(8, 2, 2, 3).Positions(),
                .. new BoundingBox(6, 5, 4, 4).Positions(),
            ],
            Figures = [
                new Positioned<Figure>(new Figure(2, 1), new Position(3, 2)),
                new Positioned<Figure>(new Figure(2, 3), new Position(8, 2)),
                new Positioned<Figure>(new Figure(2, 2), new Position(1, 4)),
                new Positioned<Figure>(new Figure(3, 3), new Position(7, 5)),
            ]
        };

        actual.ShouldBe(expectedBoard, $"Expected: \n\n{expectedBoard.ToAscii()}\n\nActual:\n{actual.ToAscii()}");
    }
}