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

    [Fact]
    public void parse_no_figure()
    {
        FigureInfo figureInfo = new NoFigure();
        bool wasSuccess = FigureInfoExtensions.TryParseFigureInfo(figureInfo.ToString(), out FigureInfo? parsed);
        wasSuccess.ShouldBeTrue();
        parsed.ShouldBe(figureInfo);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 3)]
    public void parse_figure(int width, int height)
    {
        FigureInfo figureInfo = new Figure() { Width = width, Height = height };
        bool wasSuccess = FigureInfoExtensions.TryParseFigureInfo(figureInfo.ToString(), out FigureInfo? parsed);
        wasSuccess.ShouldBeTrue();
        parsed.ShouldBe(figureInfo);
    }
    // {Figure { Width = 1, Height = 1 }}
}