#nullable disable

using Microsoft.Xna.Framework;
using Nett;

namespace Tetrominoes.Options
{
    public class GameAudioOptions
    {
        public static GameAudioOptions CreateDefault() =>
            new GameAudioOptions
            {
                MusicVolume = 15,
                SoundVolume = 75,
            };

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

        internal void ValidateConstraints()
        {
        }
    }
}
