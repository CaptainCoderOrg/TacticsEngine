namespace CaptainCoder.TacticsEngine.Board;

public abstract record PropInfo
{
    public static NoProp None { get; } = new NoProp();
}
public record NoProp : PropInfo;