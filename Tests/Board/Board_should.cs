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

    [Fact]
    public void should_have_position_to_tile_mapping()
    {
        Board underTest = new();
        Position[] positions = [
            new Position(0, 0),
            new Position(0, 1),
            new Position(0, 2),
            new Position(1, 0),
            new Position(1, 1),
            new Position(1, 2),
            new Position(2, 0),
            new Position(2, 1),
            new Position(2, 2),
        ];
        underTest.CreateEmptyTiles(positions);

        IReadOnlyDictionary<Position, TileInfo> actual = underTest.Tiles;

        actual.Count.ShouldBe(9);
        actual.Keys.ShouldBeSubsetOf(positions);
    }

    [Theory]
    [InlineData(2, 1)]
    [InlineData(1, 2)]
    public void should_remove_tile_at_position(int x, int y)
    {
        Board underTest = new();
        underTest.CreateEmptyTiles(new BoundingBox(new Position(0, 0), 3, 3).Positions());

        underTest.RemoveTile(x, y);

        underTest.Tiles.Count.ShouldBe(8);
        underTest.Tiles.Keys.ShouldNotContain(new Position(x, y));
    }

    [Fact]
    public void should_be_equals()
    {
        Board board0 = CreateBoard();
        Board board1 = CreateBoard();
        board0.ShouldBe(board1);

        static Board CreateBoard()
        {
            Board board = new();
            board.CreateEmptyTiles(
                new BoundingBox(new Position(0, 0), 7, 5).Positions()
            );
            board.AddFigure(0, 0, new Figure());
            board.AddFigure(3, 3, new Figure() { Width = 2, Height = 2 });
            return board;
        }
    }

    [Fact]
    public void should_not_equal_null()
    {
        Board board0 = new();
        board0.Equals(null).ShouldBeFalse();
    }

    [Fact]
    public void should_not_be_equals()
    {
        Board board0 = new();
        board0.CreateEmptyTiles(
            new BoundingBox(new Position(0, 0), 5, 7).Positions()
        );
        board0.AddFigure(0, 0, new Figure());
        board0.AddFigure(3, 3, new Figure() { Width = 2, Height = 2 });

        Board board1 = new();
        board1.CreateEmptyTiles(
            new BoundingBox(new Position(0, 0), 7, 5).Positions()
        );
        board1.AddFigure(3, 3, new Figure());
        board1.AddFigure(0, 0, new Figure() { Width = 2, Height = 2 });

        board0.ShouldNotBe(board1);
    }

    [Fact]
    public void should_be_jsonable()
    {
        Board board0 = CreateBoard();
        string json = board0.ToJson();
        bool wasSuccess = BoardExtensions.TryFromJson(json, out Board? deserialized);
        wasSuccess.ShouldBeTrue();
        board0.ShouldBe(deserialized);

        static Board CreateBoard()
        {
            Board board = new();
            board.CreateEmptyTiles(
                new BoundingBox(new Position(0, 0), 7, 5).Positions()
            );
            board.AddFigure(0, 0, new Figure());
            board.AddFigure(3, 3, new Figure() { Width = 2, Height = 2 });
            return board;
        }
    }

}