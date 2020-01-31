using Microsoft.Xna.Framework.Input;
using Nett;
using System;
using Tetrominoes.Input;

namespace Tetrominoes.Options
{
    public class GameInputKeyboardOptions : IInput<Keys>
    {
        public GameInputKeyboardOptions()
        {
            Enabled = true;
            Up = Keys.W;
            Down = Keys.S;
            Left = Keys.A;
            Right = Keys.D;
            RotateLeft = Keys.J;
            RotateRight = Keys.L;
            Drop = Keys.K;
            Swap = Keys.I;
            Pause = Keys.Space;
        }

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

        public static GameInputKeyboardOptions FromToml(TomlTable table)
        {
            if (table == default) throw new ArgumentNullException(nameof(table));

            var options = new GameInputKeyboardOptions();

            if (table.TryGetTable("input", out var input) &&
                input.TryGetTable("keyboard", out var keyboard))
            {
                if (keyboard.TryGetValue("enabled", out bool enabled))
                    options.Enabled = enabled;
                if (keyboard.TryGetEnumValue<Keys>("up", out var up))
                    options.Up = up;
                if (keyboard.TryGetEnumValue<Keys>("down", out var down))
                    options.Down = down;
                if (keyboard.TryGetEnumValue<Keys>("left", out var left))
                    options.Left = left;
                if (keyboard.TryGetEnumValue<Keys>("right", out var right))
                    options.Right = right;
                if (keyboard.TryGetEnumValue<Keys>("rotate_left", out var rotateLeft))
                    options.RotateLeft = rotateLeft;
                if (keyboard.TryGetEnumValue<Keys>("rotate_right", out var rotateRight))
                    options.RotateRight = rotateRight;
                if (keyboard.TryGetEnumValue<Keys>("drop", out var drop))
                    options.Drop = drop;
                if (keyboard.TryGetEnumValue<Keys>("swap", out var swap))
                    options.Swap = swap;
                if (keyboard.TryGetEnumValue<Keys>("pause", out var pause))
                    options.Pause = pause;
            }

            return options;
        }
    }
}
