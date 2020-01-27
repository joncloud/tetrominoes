using Microsoft.Xna.Framework.Media;
using System;

namespace Tetrominoes.Audio
{
    public class MusicTrack
    {
        public Song Song { get; }
        public MusicTrack(Song song) => 
            Song = song ?? throw new ArgumentNullException(nameof(song));
    }
}
