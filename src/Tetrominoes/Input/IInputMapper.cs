namespace Tetrominoes.Input
{
    public interface IInputMapper
    {
        InputState Current { get; }
        InputConnection GetConnectionState();
        InputConnection Update();
    }
}
