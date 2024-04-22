using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.RegularExpressions;

using CaptainCoder.RegexExtensions;

namespace CaptainCoder.TacticsEngine.Board;
[TypeConverter(typeof(PositionConverter))]
public record struct Position(int X, int Y);

public partial class PositionConverter : TypeConverter
{
    [GeneratedRegex(@"Position{ X = (?<X> \d+), Y = (?<Y> \d+) }", RegexOptions.IgnorePatternWhitespace)]
    public static partial Regex PositionRegex();
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        if (sourceType == typeof(string)) { return true; }
        return base.CanConvertFrom(context, sourceType);
    }

    public override bool CanConvertTo(ITypeDescriptorContext? context, [NotNullWhen(true)] Type? destinationType)
    {
        if (destinationType == typeof(string)) { return true; }
        return base.CanConvertTo(context, destinationType);
    }

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is string asString && PositionRegex().TryMatch(asString, out Match? match))
        {
            return new Position(int.Parse(match.Groups["X"].Value), int.Parse(match.Groups["Y"].Value));
        }
        return base.ConvertFrom(context, culture, value);
    }

    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        if (destinationType == typeof(string) && value is Position position)
        {
            return position.ToString();
        }
        return base.ConvertTo(context, culture, value, destinationType);
    }


    // public override bool CanConvertFrom(ITypeDescriptorContext context,
    // Type sourceType)
    // {


    // }
}