using Microsoft.Xna.Framework.Graphics;

namespace Tetrominoes
{
    public interface IBackgroundService
    {
        RenderTarget2D Board { get; }
        BackgroundEffect BackgroundEffect { get; }

        void Clear();
        void NextEffect();
    }
}
