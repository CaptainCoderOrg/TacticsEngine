using System.Text;

using CaptainCoder.TacticsEngine.Board;

namespace CaptainCoder.Tactics.Board;

/// <summary>
/// Tools for converting boards to and from ASCII. ASCII is not capable of representing all possible
/// boards but is useful in many situations.
/// </summary>
public static class AsciiBoardExtensions
{

    public static BoardData ToBoardData(this string asciiBoard)
    {
        Dictionary<char, Positioned<Figure>> figures = [];
        HashSet<Position> tiles = new();
        string[] rows = asciiBoard.ReplaceLineEndings().Split(Environment.NewLine);
        for (int y = 0; y < rows.Length; y++)
        {
            for (int x = 0; x < rows[y].Length; x++)
            {
                char ch = rows[y][x];
                if (ch == '#' || char.IsAsciiLetter(ch)) { tiles.Add(new Position(x, y)); }
                if (char.IsAsciiLetter(ch))
                {
                    AddFigure(ch, new Position(x, y));
                }
            }
        }

        return new BoardData()
        {
            Tiles = tiles,
            Figures = [.. figures.Values],
        };

        void AddFigure(char ch, Position position)
        {
            if (!figures.TryGetValue(ch, out Positioned<Figure>? positioned))
            {
                positioned = new Positioned<Figure>(new Figure(), position);
            }
            BoundingBox box = position.CreateBoundingBox(positioned.Position);
            Figure newFigure = positioned.Element with { Width = box.Width, Height = box.Height };
            figures[ch] = positioned with { Element = newFigure };
        }
    }

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