using Microsoft.Xna.Framework.Media;
using System;

namespace Tetrominoes.Audio
{
    public class OggSongAdapter : ISongAdapter
    {
        readonly Song _song;
        readonly Func<Song, int> _getSourceId;
        readonly Action<int, int, float> _setSource;
        public OggSongAdapter(Song song, Func<Song, int> getSourceId, Action<int, int, float> setSource)
        {
            _song = song ?? throw new ArgumentNullException(nameof(song));
            _getSourceId = getSourceId ?? throw new ArgumentNullException(nameof(getSourceId));
            _setSource = setSource ?? throw new ArgumentNullException(nameof(setSource));
        }

        static float XnaPitchToAlPitch(float xnaPitch)
        {
            // Copied from MonoGame SoundEffectInstance.XnaPitchToAlPitch
            return (float)Math.Pow(2.0, (double)xnaPitch);
        }

        public void SetPitch(float value)
        {
            var openAlPitch = XnaPitchToAlPitch(value);

            var sourceId = _getSourceId(_song);

            _setSource(sourceId, 4099, openAlPitch);
        }
    }
}
