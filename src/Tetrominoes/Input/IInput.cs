namespace Tetrominoes.Input
{
    public interface IInput<TValue>
    {
        TValue Up { get; }
        TValue Down { get; }
        TValue Left { get; }
        TValue Right { get; }
        TValue RotateLeft { get; }
        TValue RotateRight { get; }
        TValue Drop { get; }
        TValue Swap { get; }
        TValue Pause { get; }
        TValue Back { get; }
    }
}
