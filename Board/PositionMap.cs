using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CaptainCoder.TacticsEngine.Board;

public sealed class PositionMap<T> : IReadOnlySet<Positioned<T>>, IEquatable<PositionMap<T>> where T : IHasSize
{
    private readonly HashSet<Positioned<T>> _elements = [];
    private readonly Dictionary<Position, Positioned<T>> _occupied = [];
    public bool IsOccupied(Position position) => _occupied.ContainsKey(position);
    public bool IsOccupied(int x, int y) => IsOccupied(new Position(x, y));
    public Positioned<T>? GetValueOrDefault(Position position) => _occupied.GetValueOrDefault(position);
    public bool TryRemove(Position position, [NotNullWhen(true)] out Positioned<T>? removed)
    {
        if (!_occupied.TryGetValue(position, out removed)) { return false; }
        removed.BoundingBox().Positions().ToList().ForEach(position => _occupied.Remove(position));
        _elements.Remove(removed);
        return true;
    }
    public void Add(Positioned<T> toAdd) => Add(toAdd.Position, toAdd.Element);
    public void Add(Position position, T element)
    {
        Positioned<T> toAdd = new(element, position);
        if (toAdd.BoundingBox().Positions().Any(_occupied.ContainsKey)) { throw new InvalidOperationException($"Overlapping elements around: {toAdd.BoundingBox()}"); }
        _elements.Add(toAdd);
        toAdd.BoundingBox().Positions().ToList().ForEach(position => _occupied.Add(position, toAdd));
    }
    public bool TryAdd(Position position, T element)
    {
        Positioned<T> toAdd = new(element, position);
        if (toAdd.BoundingBox().Positions().Any(_occupied.ContainsKey)) { return false; }
        _elements.Add(toAdd);
        toAdd.BoundingBox().Positions().ToList().ForEach(position => _occupied.Add(position, toAdd));
        return true;
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