using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace CaptainCoder.RegexExtensions;

public static class RegexExtensions
{
    public static bool TryMatch(this Regex regex, string toMatch, [NotNullWhen(true)] out Match? match)
    {
        match = regex.Match(toMatch);
        return match.Success;
    }
}