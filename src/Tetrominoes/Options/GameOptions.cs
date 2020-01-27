#nullable disable

using Nett;
using System;
using System.IO;
using System.Reflection;

namespace Tetrominoes.Options
{
    public class GameOptions
    {
        public static GameOptions CreateDefault() =>
            new GameOptions
            {
                Graphics = GameGraphicsOptions.CreateDefault(),
                Input = GameInputOptions.CreateDefault()
            };

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

            if (!File.Exists(path)) return CreateDefault();

            using var stream = File.OpenRead(path);
            return Load(stream) ?? CreateDefault();
        }

        public static GameOptions Load(Stream source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            try
            {
                var options = Toml.ReadStream<GameOptions>(source)
                    ?? CreateDefault();
                options.ValidateConstraints();
                return options;
            }
            catch // TODO better handling here.
            {
                return CreateDefault();
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

        public void ValidateConstraints()
        {
            if (Graphics == null)
            {
                Graphics = GameGraphicsOptions.CreateDefault();
            }
            else
            {
                Graphics.ValidateConstraints();
            }

            if (Input == null)
            {
                Input = GameInputOptions.CreateDefault();
            }
            else
            {
                Input.ValidateConstraints();
            }

            if (Audio == null)
            {
                Audio = GameAudioOptions.CreateDefault();
            }
            else
            {
                Audio.ValidateConstraints();
            }
        }
    }
}
