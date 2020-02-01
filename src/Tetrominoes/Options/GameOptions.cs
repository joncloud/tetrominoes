using Nett;
using System;
using System.IO;
using System.Reflection;

namespace Tetrominoes.Options
{
    public class GameOptions
    {
        public GameOptions()
        {
            Gameplay = new GameGameplayOptions();
            Graphics = new GameGraphicsOptions();
            Input = new GameInputOptions();
            Audio = new GameAudioOptions();
        }

        public GameOptions(
            GameGameplayOptions gameplay,
            GameGraphicsOptions graphics,
            GameInputOptions input,
            GameAudioOptions audio
        )
        {
            Gameplay = gameplay ?? throw new ArgumentNullException(nameof(gameplay));
            Graphics = graphics ?? throw new ArgumentNullException(nameof(graphics));
            Input = input ?? throw new ArgumentNullException(nameof(input));
            Audio = audio ?? throw new ArgumentNullException(nameof(audio));
        }

        [TomlMember(Key = "gameplay")]
        public GameGameplayOptions Gameplay { get; }

        [TomlMember(Key = "graphics")]
        public GameGraphicsOptions Graphics { get; }

        [TomlMember(Key = "input")]
        public GameInputOptions Input { get; }

        [TomlMember(Key = "audio")]
        public GameAudioOptions Audio { get; }

        static string GetTomlPath()
        {
            var executingAssemblyLocation = Assembly
                .GetExecutingAssembly()
                .Location;
            var executingDirectory = Path.GetDirectoryName(
                executingAssemblyLocation
            );
            return Path.Combine(
                executingDirectory ?? Environment.CurrentDirectory,
                "config.toml"
            );
        }

        public static GameOptions FromConfigOrDefault()
        {
            var path = GetTomlPath();

            if (!File.Exists(path)) return new GameOptions();

            using var stream = File.OpenRead(path);
            return Load(stream) ?? new GameOptions();
        }

        public static GameOptions? Load(Stream source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            try
            {
                var table = Toml.ReadStream(source);

                return new GameOptions(
                    GameGameplayOptions.FromToml(table),
                    GameGraphicsOptions.FromToml(table),
                    GameInputOptions.FromToml(table),
                    GameAudioOptions.FromToml(table)
                );
            }
            catch // TODO better handling here.
            {
                return default;
            }
        }

        public void Save()
        {
            var path = GetTomlPath();

            var mode = File.Exists(path) 
                ? FileMode.Truncate 
                : FileMode.CreateNew;
            using var stream = File.Open(path, mode);
            Save(stream);
        }

        public void Save(Stream target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            try
            {
                Toml.WriteStream(this, target);
            }
            catch // TODO better handling here.
            {

            }
        }
    }
}
