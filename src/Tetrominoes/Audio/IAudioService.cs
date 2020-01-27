#nullable disable

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Tetrominoes.Audio
{
    public interface IAudioService
    {
        IMusic Music { get; }
        ISound Sound { get; }
    }

    public interface ISound
    {
        Queue<SoundEffect> Queue { get; }

        void Play(Sound sound);
    }

    public class SoundService : ISound
    {
        readonly GameComponent _host;
        public SoundService(GameComponent host)
        {
            _host = host ?? throw new ArgumentNullException(nameof(host));
            Queue = new Queue<SoundEffect>();

            _soundEffects = new Dictionary<Sound, SoundEffect>();
            foreach (var sound in Enum.GetValues(typeof(Sound)).Cast<Sound>())
            {
                _soundEffects[sound] = host.Game.Content.Load<SoundEffect>(
                    Path.Combine("Sounds", sound.ToString())
                );
            }
        }

        public Queue<SoundEffect> Queue { get; }

        readonly Dictionary<Sound, SoundEffect> _soundEffects;
        public void Play(Sound sound)
        {
            _host.Enabled = true;
            Queue.Enqueue(_soundEffects[sound]);
        }
    }

    public enum Sound
    {
        Drop,
        Move,
        RotateLeft,
        RotateRight,
        RowClear
    }

    public interface IMusic
    {
        IReadOnlyList<MusicTrack> Tracks { get; }
        MusicTrack CurrentTrack { get; }

        void Play(MusicTrack track);
        void Resume();
        void Pause();
    }

    public class MusicTrack
    {
        public Song Song { get; }
        public MusicTrack(Song song) => 
            Song = song ?? throw new ArgumentNullException(nameof(song));
    }

    public class MusicService : IMusic
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

        public MusicTrack CurrentTrack { get; private set; }
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
