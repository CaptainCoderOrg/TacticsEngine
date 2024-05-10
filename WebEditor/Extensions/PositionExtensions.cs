using CaptainCoder.TacticsEngine.Board;

using Microsoft.AspNetCore.Components.Web;

namespace WebEditor.Extensions;

public static class PositionExtensions
{
    public static Position ToPosition(this DragEventArgs args, int cellSize)
    {
        int offX = (int)(args.OffsetX / cellSize);
        int offY = (int)(args.OffsetY / cellSize);
        return new Position(offX, offY);
    }

    public static Position ToPosition(this MouseEventArgs args, int cellSize)
    {
        int offX = (int)(args.OffsetX / cellSize);
        int offY = (int)(args.OffsetY / cellSize);
        return new Position(offX, offY);
    }
}