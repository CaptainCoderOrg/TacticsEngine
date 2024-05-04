namespace CaptainCoder.TacticsEngine.Board.Tests;

using Shouldly;

public class PositionMap_should
{
    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 5)]
    [InlineData(-2, -3)]
    public void be_occupied(int x, int y)
    {
        PositionMap<Figure> underTest = [
            new Positioned<Figure>(new Figure(), new Position(0, 0)),
            new Positioned<Figure>(new Figure(1, 2), new Position(1, 4)),
            new Positioned<Figure>(new Figure(2, 2), new Position(-3, -4)),
        ];

        bool actual = underTest.IsOccupied(x, y);
        actual.ShouldBeTrue();
    }

    [Fact]
    public void get_value()
    {
        Positioned<Figure> toAdd = new(new Figure(), new Position(2, 1));
        PositionMap<Figure> underTest = [toAdd];

        Positioned<Figure>? actual = underTest.GetValueOrDefault(new Position(2, 1));
        actual.ShouldBe(toAdd);
    }

    [Fact]
    public void get_default_value()
    {
        PositionMap<Figure> underTest = [];

        Positioned<Figure>? actual = underTest.GetValueOrDefault(new Position(2, 1));
        actual.ShouldBeNull();
    }

    [Fact]
    public void be_equal()
    {
        IEnumerable<Positioned<Figure>> elements = [
            new Positioned<Figure>(new Figure(), new Position(2, 3)),
            new Positioned<Figure>(new Figure(), new Position(1, 7)),
            new Positioned<Figure>(new Figure(), new Position(7, 9))
        ];
        PositionMap<Figure> first = [.. elements];
        PositionMap<Figure> second = [.. elements];
        first.Equals(second).ShouldBeTrue();
    }

    [Fact]
    public void not_equal_to_null()
    {
        PositionMap<Figure> underTest = [];
        bool actual = underTest.Equals(null);
        actual.ShouldBeFalse();
    }

    [Fact]
    public void report_on_add()
    {
        PositionMap<Figure> underTest = [];
        underTest.TryAdd(new Position(0, 0), new Figure()).ShouldBeTrue();
        underTest.TryAdd(new Position(0, 0), new Figure()).ShouldBeFalse();

        underTest.TryAdd(new Position(2, 2), new Figure(2, 2) { Width = 2, Height = 2 }).ShouldBeTrue();

        underTest.TryAdd(new Position(3, 2), new Figure(2, 2)).ShouldBeFalse();
        underTest.TryAdd(new Position(2, 3), new Figure(2, 2)).ShouldBeFalse();
        underTest.TryAdd(new Position(3, 3), new Figure(2, 2)).ShouldBeFalse();

        underTest.TryAdd(new Position(1, 1), new Figure(2, 2)).ShouldBeFalse();
        underTest.TryAdd(new Position(1, 2), new Figure(2, 2)).ShouldBeFalse();
        underTest.TryAdd(new Position(2, 2), new Figure(2, 2)).ShouldBeFalse();
    }

    [Fact]
    public void report_on_can_add()
    {
        PositionMap<Figure> underTest = [];
        underTest.CanAdd(new Position(0, 0), new Figure()).ShouldBeTrue();
        underTest.TryAdd(new Position(0, 0), new Figure()).ShouldBeTrue();
        underTest.CanAdd(new Position(0, 0), new Figure()).ShouldBeFalse();
        underTest.TryAdd(new Position(0, 0), new Figure()).ShouldBeFalse();

        underTest.CanAdd(new Position(2, 2), new Figure(2, 2)).ShouldBeTrue();
        underTest.TryAdd(new Position(2, 2), new Figure(2, 2)).ShouldBeTrue();

        underTest.CanAdd(new Position(3, 2), new Figure(2, 2)).ShouldBeFalse();
        underTest.TryAdd(new Position(3, 2), new Figure(2, 2)).ShouldBeFalse();
        underTest.CanAdd(new Position(2, 3), new Figure(2, 2)).ShouldBeFalse();
        underTest.TryAdd(new Position(2, 3), new Figure(2, 2)).ShouldBeFalse();
        underTest.CanAdd(new Position(3, 3), new Figure(2, 2)).ShouldBeFalse();
        underTest.TryAdd(new Position(3, 3), new Figure(2, 2)).ShouldBeFalse();

        underTest.CanAdd(new Position(1, 1), new Figure(2, 2)).ShouldBeFalse();
        underTest.TryAdd(new Position(1, 1), new Figure(2, 2)).ShouldBeFalse();
        underTest.CanAdd(new Position(1, 2), new Figure(2, 2)).ShouldBeFalse();
        underTest.TryAdd(new Position(1, 2), new Figure(2, 2)).ShouldBeFalse();
        underTest.CanAdd(new Position(2, 2), new Figure(2, 2)).ShouldBeFalse();
        underTest.TryAdd(new Position(2, 2), new Figure(2, 2)).ShouldBeFalse();
    }
}