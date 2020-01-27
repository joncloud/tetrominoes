using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Tetrominoes.Audio
{
    public class MusicService : IMusicService
    {
        public MusicService(ContentManager contentManager)
        {
            if (contentManager == default) throw new ArgumentNullException(nameof(contentManager));

            var path = Path.Combine(
                contentManager.RootDirectory,
                "Music"
            );

            Tracks = Directory.GetFiles(path, "*.xnb")
                .Select(name => Path.Combine("Music", Path.GetFileNameWithoutExtension(name)))
                .Select(name => contentManager.Load<Song>(name))
                .Select(name => new MusicTrack(name))
                .ToArray();
        }

        public IReadOnlyList<MusicTrack> Tracks { get; }

        public void Resume()
        {
            MediaPlayer.Resume();
        }

        public MusicTrack? CurrentTrack { get; private set; }
        public void Play(MusicTrack track)
        {
            if (track == default) throw new ArgumentNullException(nameof(track));

            if (CurrentTrack == track) return;
            MediaPlayer.Play(track.Song);
            CurrentTrack = track;
        }

        public void Pause()
        {
            MediaPlayer.Stop();
        }
    }
}
