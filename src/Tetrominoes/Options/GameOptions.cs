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
            Graphics = new GameGraphicsOptions();
            Input = new GameInputOptions();
            Audio = new GameAudioOptions();
        }

        public GameOptions(
            GameGraphicsOptions graphics,
            GameInputOptions input,
            GameAudioOptions audio
        )
        {
            Graphics = graphics ?? throw new ArgumentNullException(nameof(graphics));
            Input = input ?? throw new ArgumentNullException(nameof(input));
            Audio = audio ?? throw new ArgumentNullException(nameof(audio));
        }

        [TomlMember(Key = "graphics")]
        public GameGraphicsOptions Graphics { get; set; }

        [TomlMember(Key = "input")]
        public GameInputOptions Input { get; set; }

        [TomlMember(Key = "audio")]
        public GameAudioOptions Audio { get; set; }

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

            using var stream = File.Open(path, File.Exists(path) ? FileMode.Truncate : FileMode.CreateNew);
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
