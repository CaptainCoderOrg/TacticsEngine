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

        underTest.CreateEmptyTile(x, y);

        // Adds an empty tile
        underTest.HasTile(x, y).ShouldBeTrue();
        TileInfo actual = underTest.GetTile(x, y);
        actual.ShouldBe(new Tile() { Figure = FigureInfo.None, Prop = PropInfo.None });
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
        underTest.CreateEmptyTile(x, y);
        Figure toAdd = new() { Width = 1, Height = 1 };

        underTest.AddFigure(x, y, toAdd);

        Tile actual = (Tile)underTest.GetTile(x, y);
        actual.Figure.ShouldBe(toAdd);
    }

    [Theory]
    [InlineData(1, 2)]
    [InlineData(-2, 3)]
    public void not_allow_2_figures_in_same_position(int x, int y)
    {
        Board underTest = new();
        underTest.CreateEmptyTile(x, y);
        Figure added = new() { Width = 1, Height = 1 };
        underTest.AddFigure(x, y, added);

        Figure toAdd = new() { Width = 1, Height = 1 };

        _ = Should.Throw<InvalidOperationException>(() =>
        {
            underTest.AddFigure(x, y, toAdd);
        });
    }

    [Theory]
    [InlineData(1, 2, 2, 3)]
    [InlineData(-2, 3, 3, 2)]
    [InlineData(4, 0, 1, 2)]
    [InlineData(0, -4, 2, 2)]
    public void add_large_figure(int x, int y, int w, int h)
    {
        Board underTest = new();
        Position[] positions = [.. new BoundingBox(new Position(x, y), w, h).Positions()];
        foreach (Position position in positions)
        {
            underTest.CreateEmptyTile(position.X, position.Y);
        }
        Figure toAdd = new() { Width = w, Height = h };

        underTest.AddFigure(x, y, toAdd);

        foreach (Position position in positions)
        {
            Tile actual = (Tile)underTest.GetTile(position.X, position.Y);
            actual.Figure.ShouldBe(toAdd);
        }

    }

    [Fact]
    public void not_allow_add_figure_to_no_tile()
    {
        Board underTest = new();
        _ = Should.Throw<ArgumentOutOfRangeException>(() => underTest.AddFigure(0, 0, new Figure()));
    }

    [Fact]
    public void not_allow_partial_add_figure()
    {
        Board underTest = new();
        underTest.CreateEmptyTile(0, 0);
        underTest.CreateEmptyTile(1, 0);
        underTest.CreateEmptyTile(0, 1);
        Figure tooLarge = new() { Width = 2, Height = 2 };

        _ = Should.Throw<ArgumentOutOfRangeException>(() => underTest.AddFigure(0, 0, tooLarge));
    }

    [Fact]
    public void should_not_allow_overlapping_figures()
    {
        Board underTest = new();
        BoundingBox box = new(new Position(0, 0), 3, 3);
        underTest.CreateEmptyTiles(box.Positions());
        Figure first = new() { Width = 2, Height = 2 };
        underTest.AddFigure(0, 0, first);
        Figure second = new() { Width = 2, Height = 2 };

        _ = Should.Throw<InvalidOperationException>(() => underTest.AddFigure(1, 1, second));
    }

}