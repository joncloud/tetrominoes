using Nett;
using System;

namespace Tetrominoes.Options
{
    public class GameGraphicsOptions
    {
        public static class Defaults
        {
            public const int Width = 1920;
            public const int Height = 1080;
            public const bool Fullscreen = true;
        }
        public GameGraphicsOptions()
        {
            Width = Defaults.Width;
            Height = Defaults.Height;
            Fullscreen = Defaults.Fullscreen;
        }

        public static GameGraphicsOptions FromToml(TomlTable table)
        {
            if (table == default) throw new ArgumentNullException(nameof(table));

            var options = new GameGraphicsOptions();

            if (table.TryGetTable("graphics", out var graphics))
            {
                if (graphics.TryGetValue("width", out uint width) && width > 0)
                {
                    options.Width = width;
                }
                if (graphics.TryGetValue("height", out uint height) && height > 0)
                {
                    options.Height = height;
                }
                if (graphics.TryGetValue("fullscreen", out bool fullscreen))
                {
                    options.Fullscreen = fullscreen;
                }
            }

            return options;
        }

        [TomlMember(Key = "width")]
        public uint Width { get; set; }
        [TomlMember(Key = "height")]
        public uint Height { get; set; }
        [TomlMember(Key = "fullscreen")]
        public bool Fullscreen { get; set; }
    }
}
