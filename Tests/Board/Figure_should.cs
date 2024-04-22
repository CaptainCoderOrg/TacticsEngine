namespace CaptainCoder.TacticsEngine.Board.Tests;

using Shouldly;

public class FigureInfo_should
{
    [Fact]
    public void jsonable_when_no_figure()
    {
        FigureInfo noFigure = new NoFigure();
        string json = noFigure.ToJson();
        bool wasSuccess = FigureInfoExtensions.TryFromJson(json, out FigureInfo? deserialized);
        wasSuccess.ShouldBeTrue();
        noFigure.ShouldBe(deserialized);
    }
    [Fact]
    public void jsonable_when_figure()
    {
        FigureInfo noFigure = new Figure() { Width = 7, Height = 2 };
        string json = noFigure.ToJson();
        bool wasSuccess = FigureInfoExtensions.TryFromJson(json, out FigureInfo? deserialized);
        wasSuccess.ShouldBeTrue();
        noFigure.ShouldBe(deserialized);
    }

}