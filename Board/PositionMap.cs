using System.Collections;
using System.Text.Json;
using System.Text.Json.Serialization;

using CaptainCoder.Linq;

using Optional;
using Optional.Collections;

namespace CaptainCoder.TacticsEngine.Board;

public sealed class PositionMap<T> : IReadOnlySet<Positioned<T>>, IEquatable<PositionMap<T>> where T : IHasSize
{
    private readonly HashSet<Positioned<T>> _elements = [];
    private readonly Dictionary<Position, Positioned<T>> _occupied = [];
    public bool IsOccupied(Position position) => _occupied.ContainsKey(position);
    public bool IsOccupied(int x, int y) => IsOccupied(new Position(x, y));
    public Option<Positioned<T>> GetValue(Position position) => _occupied.GetValueOrNone(position);
    public Option<Positioned<T>> Remove(Position position)
    {
        Option<Positioned<T>> removed = _occupied.GetValueOrNone(position);
        removed.MatchSome(r => r.BoundingBox().Positions().ForEach(_occupied.Remove));
        removed.MatchSome(r => _elements.Remove(r));
        return removed;
    }
    public void Add(Positioned<T> toAdd) => Add(toAdd.Position, toAdd.Element);
    public void Add(Position position, T element)
    {
        Positioned<T> toAdd = new(element, position);
        if (toAdd.BoundingBox().Positions().Any(_occupied.ContainsKey))
        {
            throw new InvalidOperationException($"Overlapping elements around: {toAdd.BoundingBox()}");
        }
        _elements.Add(toAdd);
        toAdd.BoundingBox().Positions().ToList().ForEach(position => _occupied.Add(position, toAdd));
    }
    public bool CanAdd(Position position, T element)
    {
        Positioned<T> toAdd = new(element, position);
        return !toAdd.BoundingBox().Positions().Any(_occupied.ContainsKey);
    }
    public Option<Positioned<T>> TryAdd(Position position, T element) => TryAdd(new Positioned<T>(element, position));
    public Option<Positioned<T>> TryAdd(Positioned<T> toAdd)
    {
        if (toAdd.BoundingBox().Positions().Any(_occupied.ContainsKey)) { return Option.None<Positioned<T>>(); }
        _elements.Add(toAdd);
        toAdd.BoundingBox().Positions().ForEach(position => _occupied.Add(position, toAdd));
        return toAdd.Some();
    }

    public bool Equals(PositionMap<T>? other) =>
        other is not null &&
        _elements.SetEquals(other._elements);

    #region IReadOnlySet
    public int Count => _elements.Count;
    public bool Contains(Positioned<T> item) => _elements.Contains(item);
    public IEnumerator<Positioned<T>> GetEnumerator() => _elements.GetEnumerator();
    public bool IsProperSubsetOf(IEnumerable<Positioned<T>> other) => _elements.IsProperSubsetOf(other);
    public bool IsProperSupersetOf(IEnumerable<Positioned<T>> other) => _elements.IsProperSupersetOf(other);
    public bool IsSubsetOf(IEnumerable<Positioned<T>> other) => _elements.IsSubsetOf(other);
    public bool IsSupersetOf(IEnumerable<Positioned<T>> other) => _elements.IsSupersetOf(other);
    public bool Overlaps(IEnumerable<Positioned<T>> other) => _elements.Overlaps(other);
    public bool SetEquals(IEnumerable<Positioned<T>> other) => _elements.SetEquals(other);
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_elements).GetEnumerator();
    #endregion
}

public class FigureMapConverter : PositionMapConverter<Figure>
{
    public static FigureMapConverter Shared { get; } = new();
}

public class PositionMapConverter<T> : JsonConverter<PositionMap<T>> where T : notnull, IHasSize
{
    public override PositionMap<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        Positioned<T>[]? values = JsonSerializer.Deserialize<Positioned<T>[]>(ref reader);
        return [.. values];
    }

    public override void Write(Utf8JsonWriter writer, PositionMap<T> value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value.ToArray());
    }
}