namespace CaptainCoder.TacticsEngine.Board.Tests;

using Shouldly;

public class BoundingBox_should
{
    [Fact]
    public void contain_position()
    {
        new BoundingBox(0, 0, 2, 2).Contains(new Position(0, 0)).ShouldBeTrue();
        new BoundingBox(0, 0, 2, 2).Contains(new Position(1, 0)).ShouldBeTrue();
        new BoundingBox(0, 0, 2, 2).Contains(new Position(0, 1)).ShouldBeTrue();
        new BoundingBox(0, 0, 2, 2).Contains(new Position(1, 1)).ShouldBeTrue();

        new BoundingBox(0, 0, 2, 2).Contains(new Position(2, 1)).ShouldBeFalse();
        new BoundingBox(0, 0, 2, 2).Contains(new Position(1, 2)).ShouldBeFalse();
        new BoundingBox(0, 0, 2, 2).Contains(new Position(2, 2)).ShouldBeFalse();

        new BoundingBox(1, 2, 3, 2).Contains(new Position(1, 2)).ShouldBeTrue();
        new BoundingBox(1, 2, 3, 2).Contains(new Position(2, 2)).ShouldBeTrue();
        new BoundingBox(1, 2, 3, 2).Contains(new Position(3, 2)).ShouldBeTrue();
        new BoundingBox(1, 2, 3, 2).Contains(new Position(1, 3)).ShouldBeTrue();
        new BoundingBox(1, 2, 3, 2).Contains(new Position(2, 3)).ShouldBeTrue();
        new BoundingBox(1, 2, 3, 2).Contains(new Position(3, 3)).ShouldBeTrue();

        new BoundingBox(1, 2, 3, 2).Contains(new Position(4, 3)).ShouldBeFalse();
        new BoundingBox(1, 2, 3, 2).Contains(new Position(3, 4)).ShouldBeFalse();

    }

}