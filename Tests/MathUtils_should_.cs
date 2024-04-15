namespace Tests;

using Model;

using Shouldly;

public class UnitTest1
{
    [Theory]
    [InlineData(1, 1, 2)]
    [InlineData(3, 5, 8)]
    [InlineData(-2, 7, 5)]
    public void add_integers(int x, int y, int sum)
    {
        MathUtils.Add(x, y).ShouldBe(sum);
    }
}