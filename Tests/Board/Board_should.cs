namespace CaptainCoder.TacticsEngine.Board.Tests;

using Optional;
using Optional.Unsafe;

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
        Option<Tile> actual = underTest.GetTile(x, y);
        actual.ValueOrDefault().ShouldBe(new Tile());
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 2)]
    [InlineData(-1, -3)]
    public void retrieve_no_tile(int x, int y)
    {
        Board underTest = new();

        Option<Tile> actual = underTest.GetTile(x, y);

        actual.HasValue.ShouldBeFalse();
    }

    [Theory]
    [InlineData(1, 2)]
    [InlineData(-2, 3)]
    public void add_1x1_figure(int x, int y)
    {
        Board underTest = new();
        underTest.CreateEmptyTile(x, y);
        Figure toAdd = new(1, 1);

        underTest.TryAddFigure(x, y, toAdd);

        Option<Tile> actual = underTest.GetTile(x, y);
        actual.ValueOrDefault().Figure.ValueOrDefault().ShouldBe(toAdd);
    }

    [Theory]
    [InlineData(1, 2)]
    [InlineData(-2, 3)]
    public void not_allow_2_figures_in_same_position(int x, int y)
    {
        Board underTest = new();
        underTest.CreateEmptyTile(x, y);
        Figure added = new(1, 1);
        underTest.TryAddFigure(x, y, added);

        Figure toAdd = new(1, 1);
        underTest.CanAddFigure(x, y, toAdd).ShouldBeFalse();
        underTest.TryAddFigure(x, y, toAdd).ShouldBeFalse();
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
        Figure toAdd = new(w, h);

        underTest.TryAddFigure(x, y, toAdd);

        foreach (Position position in positions)
        {
            Option<Tile> actual = underTest.GetTile(position.X, position.Y);
            actual.ValueOrDefault().Figure.ValueOrDefault().ShouldBe(toAdd);
        }

    }

    [Fact]
    public void not_allow_add_figure_to_no_tile()
    {
        Board underTest = new();
        underTest.CanAddFigure(0, 0, new Figure()).ShouldBeFalse();
        underTest.TryAddFigure(0, 0, new Figure()).ShouldBeFalse();
    }

    [Fact]
    public void not_allow_partial_add_figure()
    {
        Board underTest = new();
        underTest.CreateEmptyTile(0, 0);
        underTest.CreateEmptyTile(1, 0);
        underTest.CreateEmptyTile(0, 1);
        Figure tooLarge = new() { Width = 2, Height = 2 };
        underTest.CanAddFigure(0, 0, tooLarge).ShouldBeFalse();
        underTest.TryAddFigure(0, 0, tooLarge).ShouldBeFalse();
    }

    [Fact]
    public void not_allow_overlapping_figures()
    {
        Board underTest = new();
        BoundingBox box = new(new Position(0, 0), 3, 3);
        underTest.CreateEmptyTiles(box.Positions());
        Figure first = new(2, 2);
        underTest.TryAddFigure(0, 0, first);
        Figure second = new(2, 2);
        underTest.CanAddFigure(1, 1, second).ShouldBeFalse();
        underTest.TryAddFigure(1, 1, second).ShouldBeFalse();

    }

    [Theory]
    [InlineData(1, 2)]
    [InlineData(5, 7)]
    public void not_have_figure_on_empty_tile(int x, int y)
    {
        Board underTest = new();
        Position[] positions = [.. new BoundingBox(new Position(0, 0), 10, 10).Positions()];
        underTest.CreateEmptyTiles(positions);
        underTest.TryAddFigure(0, 0, new Figure());

        Option<Tile> actual = underTest.GetTile(x, y);
        actual.ValueOrDefault().ShouldBe(new Tile());
    }

    [Fact]
    public void have_position_to_tile_mapping()
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

        HashSet<Position> actual = underTest.Tiles;

        actual.Count.ShouldBe(9);
        actual.ShouldBeSubsetOf(positions);
    }

    [Theory]
    [InlineData(2, 1)]
    [InlineData(1, 2)]
    public void remove_tile_at_position(int x, int y)
    {
        Board underTest = new();
        underTest.CreateEmptyTiles(new BoundingBox(new Position(0, 0), 3, 3).Positions());

        underTest.RemoveTile(x, y);

        underTest.Tiles.Count.ShouldBe(8);
        underTest.Tiles.ShouldNotContain(new Position(x, y));
    }

    [Theory]
    [InlineData(2, 1)]
    [InlineData(1, 2)]
    public void remove_tile_and_figure(int x, int y)
    {
        Board underTest = new()
        {
            Tiles = [.. new BoundingBox(0, 0, 3, 3).Positions()],
            Figures = [new Positioned<Figure>(new Figure(), new Position(x, y))]
        };

        underTest.RemoveTile(x, y);

        underTest.Tiles.Count.ShouldBe(8);
        underTest.Tiles.ShouldNotContain(new Position(x, y));
        underTest.Figures.Count.ShouldBe(0);
    }

    [Fact]
    public void be_equals()
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
            board.TryAddFigure(0, 0, new Figure());
            board.TryAddFigure(3, 3, new Figure(2, 2));
            return board;
        }
    }

    [Fact]
    public void not_equal_null()
    {
        Board board0 = new();
        board0.Equals(null).ShouldBeFalse();
    }

    [Fact]
    public void not_be_equals()
    {
        Board board0 = new();
        board0.CreateEmptyTiles(
            new BoundingBox(new Position(0, 0), 5, 7).Positions()
        );
        board0.TryAddFigure(0, 0, new Figure());
        board0.TryAddFigure(3, 3, new Figure(2, 2));

        Board board1 = new();
        board1.CreateEmptyTiles(
            new BoundingBox(new Position(0, 0), 7, 5).Positions()
        );
        board1.TryAddFigure(3, 3, new Figure());
        board1.TryAddFigure(0, 0, new Figure(2, 2));

        board0.ShouldNotBe(board1);
    }

    [Fact]
    public void be_jsonable()
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
            board.TryAddFigure(0, 0, new Figure());
            board.TryAddFigure(3, 3, new Figure(2, 2));
            return board;
        }
    }


    [Theory]
    [InlineData(1, 2)]
    [InlineData(2, 2)]
    [InlineData(3, 2)]
    [InlineData(1, 3)]
    [InlineData(2, 3)]
    [InlineData(3, 3)]
    [InlineData(1, 4)]
    [InlineData(2, 4)]
    [InlineData(3, 4)]
    public void remove_figure(int x, int y)
    {
        Figure figure = new(3, 3);
        Figure other = new(2, 2);
        Board underTest = new()
        {
            Tiles = [.. new BoundingBox(0, 0, 10, 10).Positions()],
            Figures = [
                new Positioned<Figure>(figure, new Position(1, 2)),
                new Positioned<Figure>(other, new Position(5, 3)),
            ]
        };

        bool result = underTest.RemoveFigure(new Position(x, y));

        result.ShouldBeTrue();
        underTest.Figures.Count.ShouldBe(1);
        underTest.Figures.First().ShouldBe(new Positioned<Figure>(other, new Position(5, 3)));
    }

    [Theory]
    [InlineData(1, 2)]
    [InlineData(2, 2)]
    [InlineData(2, 1)]
    public void not_remove_figure(int x, int y)
    {
        Figure other = new(2, 2);
        Board underTest = new()
        {
            Tiles = [.. new BoundingBox(0, 0, 10, 10).Positions()],
            Figures = [
                new Positioned<Figure>(other, new Position(5, 3)),
            ]
        };

        bool result = underTest.RemoveFigure(new Position(x, y));

        result.ShouldBeFalse();
        underTest.Figures.Count.ShouldBe(1);
        underTest.Figures.First().ShouldBe(new Positioned<Figure>(other, new Position(5, 3)));
    }

}