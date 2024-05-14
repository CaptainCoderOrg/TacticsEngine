namespace CaptainCoder.TacticsEngine.Board.Tests;

using CaptainCoder.Tactics.Board;

using Shouldly;

public class BoardSelection_should
{
    public const string SampleAsciiBoard = """
    .......... 0
    .#####.... 1
    .##AA#..## 2
    .#####..## 3
    .BB###..CC 4
    .BB###.... 5
    ......#### 6
    ......###D 7
    0123456789
    """;

    private readonly BoardData _underTest;

    public BoardSelection_should()
    {
        _underTest = SampleAsciiBoard.ToBoardData();
    }

    [Fact]
    public void contain_empty_board()
    {
        BoardData actualSelected = _underTest.GetSelection(new BoundingBox(7, 0, 3, 2));

        BoardData expectedBoard = new();
        actualSelected.ShouldBe(expectedBoard, ErrorMessage(actualSelected, expectedBoard));
    }

    [Fact]
    public void contain_3x3_square()
    {
        BoardData actualSelected = _underTest.GetSelection(new BoundingBox(3, 3, 3, 3));

        BoardData expectedBoard = """
        ###
        ###
        ###
        """.ToBoardData();

        actualSelected.ShouldBe(expectedBoard, ErrorMessage(actualSelected, expectedBoard));
    }

    [Fact]
    public void remove_3x3_square()
    {
        _underTest.RemoveSelection(new BoundingBox(3, 3, 3, 3));

        BoardData expectedBoard = """
        .......... 0
        .#####.... 1
        .##AA#..## 2
        .##.....## 3
        .BB.....CC 4
        .BB....... 5
        ......#### 6
        ......###D 7
        0123456789
        """.ToBoardData();

        _underTest.ShouldBe(expectedBoard, ErrorMessage(_underTest, expectedBoard));
    }

    [Fact]
    public void contain_figure()
    {
        BoardData actualSelected = _underTest.GetSelection(new BoundingBox(2, 2, 4, 2));

        BoardData expectedBoard = """
        #AA#
        ####
        """.ToBoardData();

        actualSelected.ShouldBe(expectedBoard, ErrorMessage(actualSelected, expectedBoard));
    }

    [Fact]
    public void remove_selection_with_figure_square()
    {
        _underTest.RemoveSelection(new BoundingBox(2, 2, 4, 2));

        BoardData expectedBoard = """
        .......... 0
        .#####.... 1
        .#......## 2
        .#......## 3
        .BB###..CC 4
        .BB###.... 5
        ......#### 6
        ......###D 7
        0123456789
        """.ToBoardData();

        _underTest.ShouldBe(expectedBoard, ErrorMessage(_underTest, expectedBoard));
    }

    [Fact]
    public void keep_tiles_with_partial_figures()
    {
        BoardData actualSelected = _underTest.GetSelection(new BoundingBox(2, 1, 2, 4));

        BoardData expectedBoard = """
         .##.
         .#AA
         .##.
         BB#.
         BB..
         """.ToBoardData();

        actualSelected.ShouldBe(expectedBoard, ErrorMessage(actualSelected, expectedBoard));
    }

    [Fact]
    public void remove_selection_with_partial_figure_square()
    {
        _underTest.RemoveSelection(new BoundingBox(2, 1, 2, 4));

        BoardData expectedBoard = """
        .......... 0
        .#..##.... 1
        .#..##..## 2
        .#..##..## 3
        .#..##..CC 4
        .#####.... 5
        ......#### 6
        ......###D 7
        0123456789
        """.ToBoardData();

        _underTest.ShouldBe(expectedBoard, ErrorMessage(_underTest, expectedBoard));
    }

    private string ErrorMessage(BoardData actual, BoardData expected)
    {
        return $"""
         Expected:

         {expected.ToAscii()}

         But was:

         {actual.ToAscii()}
         """;
    }
}