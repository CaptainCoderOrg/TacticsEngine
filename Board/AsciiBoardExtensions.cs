using System.Text;

using CaptainCoder.TacticsEngine.Board;

namespace CaptainCoder.Tactics.Board;

/// <summary>
/// Tools for converting boards to and from ASCII. ASCII is not capable of representing all possible
/// boards but is useful in many situations.
/// </summary>
public static class AsciiBoardExtensions
{
    public static string ToAscii(this BoardData board)
    {
        BoundingBox bbox = board.BoundingBox();

        char nextChar = 'A';
        Dictionary<Positioned<Figure>, char> charLookup = [];

        StringBuilder builder = new();
        for (int row = 0; row <= bbox.Height; row++)
        {
            for (int col = 0; col <= bbox.Width; col++)
            {
                _ = board.TryGetTile(col, row, out Tile? tile);
                _ = builder.Append(ToChar(tile));
            }
            builder.AppendLine();
        }
        return builder.ToString().TrimEnd();

        char ToChar(Tile? tile) => tile switch
        {
            null => '.',
            Tile t when t.Figure is null => '#',
            Tile t when t.Figure is Positioned<Figure> figure => GetChar(figure),
            _ => throw new NotImplementedException(),
        };

        char GetChar(Positioned<Figure> figure)
        {
            if (charLookup.TryGetValue(figure, out char value)) { return value; }
            charLookup[figure] = nextChar++;
            return charLookup[figure];
        }

    }
}