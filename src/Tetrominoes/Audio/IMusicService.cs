using System.Collections.Generic;

namespace Tetrominoes.Audio
{
    public interface IMusicService
    {
        IReadOnlyList<MusicTrack> Tracks { get; }
        MusicTrack? CurrentTrack { get; }

        void Play(MusicTrack track);
        void Resume();
        void Pause();
    }
}
