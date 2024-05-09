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
        BoardData underTest = new();

        underTest.CreateEmptyTile(x, y);

        // Adds an empty tile
        underTest.HasTile(x, y).ShouldBeTrue();
        Tile actual = underTest[x, y];
        actual.ShouldBe(new Tile());
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 2)]
    [InlineData(-1, -3)]
    public void retrieve_no_tile(int x, int y)
    {
        BoardData underTest = new();

        underTest.TryGetTile(x, y, out Tile? _).ShouldBeFalse();
    }

    [Theory]
    [InlineData(1, 2)]
    [InlineData(-2, 3)]
    public void add_1x1_figure(int x, int y)
    {
        BoardData underTest = new();
        underTest.CreateEmptyTile(x, y);
        Figure toAdd = new(1, 1);

        underTest.TryAddFigure(x, y, toAdd);

        Tile actual = underTest[x, y];
        actual.ShouldBe(new Tile() { Figure = new Positioned<Figure>(toAdd, new Position(x, y)) });
    }

    [Theory]
    [InlineData(1, 2)]
    [InlineData(-2, 3)]
    public void not_allow_2_figures_in_same_position(int x, int y)
    {
        BoardData underTest = new();
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
        BoardData underTest = new();
        Position[] positions = [.. new BoundingBox(new Position(x, y), w, h).Positions()];
        foreach (Position position in positions)
        {
            underTest.CreateEmptyTile(position.X, position.Y);
        }
        Figure toAdd = new(w, h);

        underTest.TryAddFigure(x, y, toAdd).ShouldBeTrue();

        foreach (Position position in positions)
        {
            Tile actual = underTest[position];
            actual.Figure.ShouldBe(new Positioned<Figure>(toAdd, new Position(x, y)));
        }

    }

    [Fact]
    public void not_allow_add_figure_to_no_tile()
    {
        BoardData underTest = new();
        underTest.CanAddFigure(0, 0, new Figure()).ShouldBeFalse();
        underTest.TryAddFigure(0, 0, new Figure()).ShouldBeFalse();
    }

    [Fact]
    public void not_allow_partial_add_figure()
    {
        BoardData underTest = new();
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
        BoardData underTest = new();
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
        BoardData underTest = new();
        Position[] positions = [.. new BoundingBox(new Position(0, 0), 10, 10).Positions()];
        underTest.CreateEmptyTiles(positions);
        underTest.TryAddFigure(0, 0, new Figure());

        Tile actual = underTest[x, y];
        actual.ShouldBe(new Tile());
    }

    [Fact]
    public void have_position_to_tile_mapping()
    {
        BoardData underTest = new();
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
        BoardData underTest = new();
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
        BoardData underTest = new()
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
        BoardData board0 = CreateBoard();
        BoardData board1 = CreateBoard();
        board0.ShouldBe(board1);

        static BoardData CreateBoard()
        {
            BoardData board = new();
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
        BoardData board0 = new();
        board0.Equals(null).ShouldBeFalse();
    }

    [Fact]
    public void not_be_equals()
    {
        BoardData board0 = new();
        board0.CreateEmptyTiles(
            new BoundingBox(new Position(0, 0), 5, 7).Positions()
        );
        board0.TryAddFigure(0, 0, new Figure());
        board0.TryAddFigure(3, 3, new Figure(2, 2));

        BoardData board1 = new();
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
        BoardData board0 = CreateBoard();
        string json = board0.ToJson();
        bool wasSuccess = BoardExtensions.TryFromJson(json, out BoardData? deserialized);
        wasSuccess.ShouldBeTrue();
        board0.ShouldBe(deserialized);

        static BoardData CreateBoard()
        {
            BoardData board = new();
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
        BoardData underTest = new()
        {
            Tiles = [.. new BoundingBox(0, 0, 10, 10).Positions()],
            Figures = [
                new Positioned<Figure>(figure, new Position(1, 2)),
                new Positioned<Figure>(other, new Position(5, 3)),
            ]
        };

        bool result = underTest.TryRemoveFigure(new Position(x, y), out Positioned<Figure>? _);

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
        BoardData underTest = new()
        {
            Tiles = [.. new BoundingBox(0, 0, 10, 10).Positions()],
            Figures = [
                new Positioned<Figure>(other, new Position(5, 3)),
            ]
        };

        bool result = underTest.TryRemoveFigure(new Position(x, y), out Positioned<Figure>? _);

        result.ShouldBeFalse();
        underTest.Figures.Count.ShouldBe(1);
        underTest.Figures.First().ShouldBe(new Positioned<Figure>(other, new Position(5, 3)));
    }


    [Fact]
    public void throw_out_of_range_exception()
    {
        BoardData underTest = new();
        Should.Throw<IndexOutOfRangeException>(() => underTest[0, 0]);
    }

    [Theory]
    [InlineData(2, 2, 2, 2, 4, 4)]
    [InlineData(1, 2, 2, 1, 3, 2)]
    [InlineData(3, 2, 3, 3, 6, 6)]
    public void move_figure(int startX, int startY, int width, int height, int endX, int endY)
    {
        Positioned<Figure> figure = new(new Figure(), new Position(startX, startY));
        BoardData board = new()
        {
            Tiles = [.. new BoundingBox(0, 0, 10, 10).Positions()],
            Figures = [figure]
        };

        bool result = board.TryMoveFigure(new Position(startX, startY), new Position(endX, endY));

        result.ShouldBeTrue();
        board.Figures.Count.ShouldBe(1);
        BoundingBox startBox = new(startX, startY, width, height);
        Positioned<Figure> movedFigure = new(new Figure(), new Position(endX, endY));
        startBox.Positions()
                .Select(board.GetTile)
                .All(new Tile().Equals)
                .ShouldBeTrue();

        BoundingBox endBox = new(endX, endY, width, height);
        board.TryGetTile(new Position(endX, endY), out Tile? tile).ShouldBeTrue();
        tile.Figure.ShouldBe(figure with { Position = new Position(endX, endY) });
    }

    [Theory]
    [InlineData(2, 2, 4, 4)]
    [InlineData(1, 2, 3, 2)]
    public void not_move_1x1_figure_when_end_is_occupied(int startX, int startY, int endX, int endY)
    {
        Positioned<Figure> figure = new(new Figure(), new Position(startX, startY));
        Positioned<Figure> other = new(new Figure(), new Position(endX, endY));
        BoardData board = new()
        {
            Tiles = [.. new BoundingBox(0, 0, 10, 10).Positions()],
            Figures = [
                figure,
                other,
            ]
        };

        bool result = board.TryMoveFigure(new Position(startX, startY), new Position(endX, endY));

        result.ShouldBeFalse();
        board.Figures.Count.ShouldBe(2);
        board.GetTile(new Position(startX, startY)).ShouldBe(new Tile() { Figure = figure });
        board.GetTile(new Position(endX, endY)).ShouldBe(new Tile() { Figure = other });
    }

    [Fact]
    public void not_move_3x3_figure_when_end_is_occupied()
    {
        Positioned<Figure> figure = new(new Figure() { Width = 3, Height = 3 }, new Position(1, 2));
        Figure other = new() { Width = 2, Height = 2 };
        BoardData board = new()
        {
            Tiles = [.. new BoundingBox(0, 0, 10, 10).Positions()],
            Figures = [
                figure,
                new Positioned<Figure>(other, new Position(5, 3)),
            ]
        };

        bool result = board.TryMoveFigure(new Position(1, 2), new Position(4, 2));

        result.ShouldBeFalse();
        board.Figures.Count.ShouldBe(2);
        board.GetTile(new Position(1, 2)).ShouldBe(new Tile() { Figure = figure });
        board.GetTile(new Position(4, 2)).ShouldBe(new Tile());
    }

    [Theory]
    [InlineData(1, 2, 2, 2)]
    [InlineData(2, 2, 3, 2)]
    [InlineData(2, 1, 4, 2)]
    public void not_move_empty_tile(int startX, int startY, int endX, int endY)
    {
        Figure other = new() { Width = 2, Height = 2 };
        BoardData underTest = new()
        {
            Tiles = [.. new BoundingBox(0, 0, 10, 10).Positions()],
            Figures = [
                new Positioned<Figure>(other, new Position(5, 3)),
            ]
        };

        bool result = underTest.TryMoveFigure(new Position(startX, startY), new Position(endX, endY));

        result.ShouldBeFalse();
        underTest.Figures.Count.ShouldBe(1);
        underTest.Figures.First().ShouldBe(new Positioned<Figure>(other, new Position(5, 3)));
    }
}