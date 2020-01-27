using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

namespace Tetrominoes.Audio
{
    public interface ISoundService
    {
        Queue<SoundEffect> Queue { get; }

        void Play(Sound sound);
    }
}
