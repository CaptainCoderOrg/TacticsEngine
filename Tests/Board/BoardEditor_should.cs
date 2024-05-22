namespace CaptainCoder.TacticsEngine.Board.Tests;

using CaptainCoder.Tactics.Board;

using Shouldly;

public class BoardEditor_should
{
    [Fact]
    public void add_tiles_at_offset()
    {
        BoardData underTest = """
            ......0
            .####.1
            .####.2
            ......3
            0123456
            """.ToBoardData();

        BoardData toAdd = """
            ##
            ##
            """.ToBoardData();

        underTest.AddAll(toAdd, new Position(6, 0));

        BoardData expected = """
            ......##..
            .####.##..
            .####.....
            ..........
            012345....
            """.ToBoardData();

        underTest.ShouldBe(expected, ErrorMessage(underTest, expected));
    }

    [Fact]
    public void add_tiles_and_figures_at_offset()
    {
        BoardData underTest = """
            ......0
            .####.1
            .####.2
            ......3
            0123456
            """.ToBoardData();

        BoardData toAdd = """
            ###
            #AA
            #AA
            """.ToBoardData();

        underTest.AddAll(toAdd, new Position(7, 1));

        BoardData expected = """
            ...........
            .####..###.
            .####..#AA.
            .......#AA.
            """.ToBoardData();

        underTest.ShouldBe(expected, ErrorMessage(underTest, expected));
    }

    [Fact]
    public void add_overlapping_tiles_and_figures_at_offset()
    {
        BoardData underTest = """
            ......0
            .##AA.1
            .##AA.2
            ......3
            0123456
            """.ToBoardData();

        BoardData toAdd = """
            BB#
            BB#
            ###
            """.ToBoardData();

        underTest.AddAll(toAdd, new Position(4, 2));

        BoardData expected = """
            .......0
            .####..1
            .###BB#2
            ....BB#3
            ....###4
            0123456
            """.ToBoardData();

        underTest.ShouldBe(expected, ErrorMessage(underTest, expected));
    }

    [Fact]
    public void copy_board()
    {
        BoardData toCopy = """
            .......0
            .A###..1
            .A##BB#2
            ....BB#3
            ....###4
            0123456
            """.ToBoardData();

        BoardData copy = toCopy.Copy();
        copy.ShouldBe(toCopy);
        object.ReferenceEquals(copy, toCopy).ShouldBeFalse();

        copy.CreateEmptyTile(0, 0);
        copy.ShouldNotBe(toCopy);
    }

    public string ErrorMessage(BoardData actual, BoardData expected)
    {
        return $"""
         Expected:

         {expected.ToAscii()}

         But was:

         {actual.ToAscii()}
         """;
    }
}