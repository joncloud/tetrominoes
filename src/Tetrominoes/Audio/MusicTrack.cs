using Microsoft.Xna.Framework.Media;
using System;

namespace Tetrominoes.Audio
{
    public class MusicTrack
    {
        readonly ISongAdapter _adapter;
        public Song Song { get; }
        public MusicTrack(Song song)
        {
            Song = song ?? throw new ArgumentNullException(nameof(song));
            _adapter = SongAdapterFactory.Create(song);
        }

        public void SetPitch(float value) => _adapter.SetPitch(value);
    }
}
