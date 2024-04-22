using System.Text.Json.Serialization;

namespace CaptainCoder.TacticsEngine.Board;

[JsonDerivedType(typeof(NoProp), typeDiscriminator: "NoProp")]
public abstract record PropInfo
{
    public static NoProp None { get; } = new NoProp();
}
public sealed record NoProp : PropInfo;