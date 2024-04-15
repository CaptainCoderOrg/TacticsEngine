namespace CaptainCoder.TacticsEngine.Board.Tests;

using Shouldly;

public class Board_should
{
    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 5)]
    [InlineData(-2, -3)]
    public void add_tiles(int x, int y)
    {
        Board underTest = new();

        underTest.AddTile(x, y);

        // Adds an empty tile
        underTest.HasTile(x, y).ShouldBeTrue();
        TileInfo actual = underTest.GetTile(x, y);
        actual.ShouldBe(new Tile() { Figure = FigureInfo.None, Prop = PropInfo.None } );
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 2)]
    [InlineData(-1, -3)]
    public void retrieve_no_tile(int x, int y)
    {
        Board underTest = new();

        TileInfo actual = underTest.GetTile(x, y);

        actual.ShouldBe(TileInfo.None);
    }

    [Theory]
    [InlineData(1, 2)]
    [InlineData(-2, 3)]
    public void add_1x1_figure(int x, int y)
    {
        Board underTest = new();
        underTest.AddTile(x, y);
        Figure toAdd = new () { Width = 1, Height = 1 };

        underTest.AddFigure(x, y, toAdd);

        Tile actual = (Tile)underTest.GetTile(x, y);
        actual.Figure.ShouldBe(toAdd);
    }


}