using Microsoft.Xna.Framework.Input;
using Nett;

namespace Tetrominoes.Options
{
    public class GameInputKeyboardOptions
    {
        public static GameInputKeyboardOptions CreateDefault() =>
            new GameInputKeyboardOptions
            {
                Enabled = true,
                Up = Keys.W,
                Down = Keys.S,
                Left = Keys.A,
                Right = Keys.D,
                RotateLeft = Keys.J,
                RotateRight = Keys.L,
                Drop = Keys.K,
                Swap = Keys.I,
                Pause = Keys.Space
            };

        [TomlMember(Key = "enabled")]
        public bool Enabled { get; set; }

        [TomlMember(Key = "up")]
        public Keys Up { get; set; }
        [TomlMember(Key = "down")]
        public Keys Down { get; set; }
        [TomlMember(Key = "left")]
        public Keys Left { get; set; }
        [TomlMember(Key = "right")]
        public Keys Right { get; set; }
        [TomlMember(Key = "rotate_left")]
        public Keys RotateLeft { get; set; }
        [TomlMember(Key = "rotate_right")]
        public Keys RotateRight { get; set; }
        [TomlMember(Key = "drop")]
        public Keys Drop { get; set; }
        [TomlMember(Key = "swap")]
        public Keys Swap { get; set; }
        [TomlMember(Key = "pause")]
        public Keys Pause { get; set; }

        internal void ValidateConstraints()
        {
            Up = EnumHelper<Keys>.IfDefined(Up, Keys.W);
            Down = EnumHelper<Keys>.IfDefined(Down, Keys.S);
            Left = EnumHelper<Keys>.IfDefined(Left, Keys.A);
            Right = EnumHelper<Keys>.IfDefined(Right, Keys.D);
            RotateLeft = EnumHelper<Keys>.IfDefined(RotateLeft, Keys.J);
            RotateRight = EnumHelper<Keys>.IfDefined(RotateRight, Keys.L);
            Drop = EnumHelper<Keys>.IfDefined(Drop, Keys.K);
            Swap = EnumHelper<Keys>.IfDefined(Swap, Keys.I);
            Pause = EnumHelper<Keys>.IfDefined(Pause, Keys.Space);
        }
    }
}
