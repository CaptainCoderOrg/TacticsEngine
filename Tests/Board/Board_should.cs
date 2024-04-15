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
        Tile actual = underTest.GetTile(x, y);
        actual.ShouldBe(new TileInfo() { Figure = Figure.None, Prop = Prop.None } );
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 2)]
    [InlineData(-1, -3)]
    public void retrieve_no_tile(int x, int y)
    {
        Board underTest = new();

        Tile actual = underTest.GetTile(x, y);

        actual.ShouldBe(Tile.None);
    }

}