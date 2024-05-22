using CaptainCoder.Tactics.Board;
using CaptainCoder.TacticsEngine.Board;

namespace Tests;

internal static class ErrorMessages
{
    public static string BoardCompareError(BoardData expected, BoardData actual)
    {
        return $"""
         Expected:

         {expected.ToAscii()}

         But was:

         {actual.ToAscii()}
         """;
    }
}