namespace CaptainCoder.TacticsEngine.Board;

public sealed record Figure(int Width = 1, int Height = 1) : IHasSize;