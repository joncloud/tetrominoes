#nullable disable

using Nett;

namespace Tetrominoes.Options
{
    public class GameGraphicsOptions
    {
        public static GameGraphicsOptions CreateDefault() =>
            new GameGraphicsOptions
            {
                Width = 1920,
                Height = 1080
            };

        [TomlMember(Key = "width")]
        public uint Width { get; set; }
        [TomlMember(Key = "height")]
        public uint Height { get; set; }
        [TomlMember(Key = "fullscreen")]
        public bool Fullscreen { get; set; }

        internal void ValidateConstraints()
        {
            if (Width <= 0)
            {
                Width = 1920;
            }
            if (Height <= 1080)
            {
                Height = 1080;
            }
        }
    }
}
