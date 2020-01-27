namespace Tetrominoes.Input
{
    public interface IInputService
    {
        bool Enabled { get; set; }
        InputState State { get; }
    }
}
