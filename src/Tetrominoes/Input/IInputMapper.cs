using Microsoft.Xna.Framework;

namespace Tetrominoes.Input
{
    public interface IInputMapper
    {
        InputState Current { get; }
        InputConnection GetConnectionState();
        InputConnection Update(GameTime gameTime);
    }
}
