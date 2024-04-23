namespace CaptainCoder.TacticsEngine.Board.Tests;

using Shouldly;

public class Position_should
{
    [Fact]
    public void get_2x1_position_rect()
    {
        Position position = new(1, 2);

        HashSet<Position> actual = [.. new BoundingBox(position, 2, 1).Positions()];

        actual.Count.ShouldBe(2);
        actual.ShouldContain(new Position(1, 2));
        actual.ShouldContain(new Position(2, 2));
    }

    [Fact]
    public void get_3x2_position_rect()
    {
        Position position = new(-1, -7);

        HashSet<Position> actual = [.. new BoundingBox(position, 3, 2).Positions()];

        actual.Count.ShouldBe(6);
        actual.ShouldContain(new Position(-1, -7));
        actual.ShouldContain(new Position(0, -7));
        actual.ShouldContain(new Position(1, -7));

        actual.ShouldContain(new Position(-1, -6));
        actual.ShouldContain(new Position(0, -6));
        actual.ShouldContain(new Position(1, -6));
    }

}