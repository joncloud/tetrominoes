using Microsoft.Xna.Framework;
using Nett;
using System;

namespace Tetrominoes.Options
{
    public class GameAudioOptions
    {
        public GameAudioOptions()
        {
            MusicVolume = 15;
            SoundVolume = 75;
        }

        int _musicVolume;

        [TomlMember(Key = "music_volume")]
        public int MusicVolume
        {
            get => _musicVolume;
            set
            {
                _musicVolume = MathHelper.Clamp(value, 0, 100);
                MusicVolumePercentage = _musicVolume / 100.0f;
            }
        }

        [TomlIgnore]
        public float MusicVolumePercentage { get; private set; }

        int _soundVolume;

        [TomlMember(Key = "sound_volume")]
        public int SoundVolume
        {
            get => _soundVolume;
            set
            {
                _soundVolume = MathHelper.Clamp(value, 0, 100);
                SoundVolumePercentage = _soundVolume / 100.0f;
            }
        }

        [TomlIgnore]
        public float SoundVolumePercentage { get; private set; }

        public static GameAudioOptions FromToml(TomlTable table)
        {
            if (table == default) throw new ArgumentNullException(nameof(table));

            var options = new GameAudioOptions();

            if (table.TryGetTable("audio", out var graphics))
            {
                if (graphics.TryGetValue("music_volume", out int width))
                {
                    options.MusicVolume = width;
                }
                if (graphics.TryGetValue("sound_volume", out int height))
                {
                    options.SoundVolume = height;
                }
            }

            return options;
        }
    }
}
