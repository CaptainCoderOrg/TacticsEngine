namespace CaptainCoder.TacticsEngine.Board;

public abstract record Prop
{
    public static NoProp None { get; } = new NoProp();
}
public record NoProp : Prop;