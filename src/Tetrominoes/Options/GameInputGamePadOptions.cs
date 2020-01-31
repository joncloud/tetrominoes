using Microsoft.Xna.Framework;
using Nett;
using System;
using Tetrominoes.Input;

namespace Tetrominoes.Options
{
    public class GameInputGamePadOptions : IInput<GamePadButtonTypes>
    {
        // CS8618 is disabled, because the *Map properties are
        // initialized through the property setters.
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        public GameInputGamePadOptions()
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        {
            Enabled = true;
            PlayerIndex = PlayerIndex.One;
            Up = GamePadButtonTypes.DPadUp;
            Down = GamePadButtonTypes.DPadDown;
            Left = GamePadButtonTypes.DPadLeft;
            Right = GamePadButtonTypes.DPadRight;
            RotateLeft = GamePadButtonTypes.ButtonsA;
            RotateRight = GamePadButtonTypes.ButtonsB;
            Drop = GamePadButtonTypes.ButtonsLeftShoulder;
            Swap = GamePadButtonTypes.ButtonsRightShoulder;
            Pause = GamePadButtonTypes.ButtonsStart;
        }

        [TomlMember(Key = "enabled")]
        public bool Enabled { get; set; }

        [TomlMember(Key = "controller")]
        public PlayerIndex PlayerIndex { get; set; }

        GamePadButtonTypes _up;
        [TomlMember(Key = "up")]
        public GamePadButtonTypes Up
        {
            get => _up;
            set
            {
                _up = value;
                UpMap = GamePadButtonMaps.GetMapFor(value);
            }
        }

        [TomlIgnore]
        public GetButtonState UpMap { get; private set; }

        GamePadButtonTypes _down;
        [TomlMember(Key = "down")]
        public GamePadButtonTypes Down
        {
            get => _down;
            set
            {
                _down = value;
                DownMap = GamePadButtonMaps.GetMapFor(value);
            }
        }
        [TomlIgnore]
        public GetButtonState DownMap { get; private set; }

        GamePadButtonTypes _left;
        [TomlMember(Key = "left")]
        public GamePadButtonTypes Left
        {
            get => _left;
            set
            {
                _left = value;
                LeftMap = GamePadButtonMaps.GetMapFor(value);
            }
        }
        [TomlIgnore]
        public GetButtonState LeftMap { get; private set; }

        GamePadButtonTypes _right;
        [TomlMember(Key = "right")]
        public GamePadButtonTypes Right
        {
            get => _right;
            set
            {
                _right = value;
                RightMap = GamePadButtonMaps.GetMapFor(value);
            }
        }
        [TomlIgnore]
        public GetButtonState RightMap { get; private set; }

        GamePadButtonTypes _rotateLeft;
        [TomlMember(Key = "rotate_left")]
        public GamePadButtonTypes RotateLeft
        {
            get => _rotateLeft;
            set
            {
                _rotateLeft = value;
                RotateLeftMap = GamePadButtonMaps.GetMapFor(value);
            }
        }
        [TomlIgnore]
        public GetButtonState RotateLeftMap { get; private set; }

        GamePadButtonTypes _rotateRight;
        [TomlMember(Key = "rotate_right")]
        public GamePadButtonTypes RotateRight
        {
            get => _rotateRight;
            set
            {
                _rotateRight = value;
                RotateRightMap = GamePadButtonMaps.GetMapFor(value);
            }
        }
        [TomlIgnore]
        public GetButtonState RotateRightMap { get; private set; }

        GamePadButtonTypes _drop;
        [TomlMember(Key = "drop")]
        public GamePadButtonTypes Drop
        {
            get => _drop;
            set
            {
                _drop = value;
                DropMap = GamePadButtonMaps.GetMapFor(value);
            }
        }
        [TomlIgnore]
        public GetButtonState DropMap { get; private set; }

        GamePadButtonTypes _swap;
        [TomlMember(Key = "swap")]
        public GamePadButtonTypes Swap
        {
            get => _swap;
            set
            {
                _swap = value;
                SwapMap = GamePadButtonMaps.GetMapFor(value);
            }
        }
        [TomlIgnore]
        public GetButtonState SwapMap { get; private set; }

        GamePadButtonTypes _pause;
        [TomlMember(Key = "pause")]
        public GamePadButtonTypes Pause
        {
            get => _pause;
            set
            {
                _pause = value;
                PauseMap = GamePadButtonMaps.GetMapFor(value);
            }
        }
        [TomlIgnore]
        public GetButtonState PauseMap { get; private set; }

        public static GameInputGamePadOptions FromToml(TomlTable table)
        {
            if (table == default) throw new ArgumentNullException(nameof(table));

            var options = new GameInputGamePadOptions();

            if (table.TryGetTable("input", out var input) &&
                input.TryGetTable("gamepad", out var gamepad))
            {
                if (gamepad.TryGetValue("enabled", out bool enabled))
                    options.Enabled = enabled;
                if (gamepad.TryGetEnumValue<PlayerIndex>("controller", out var controller))
                    options.PlayerIndex = controller;
                if (gamepad.TryGetEnumValue<GamePadButtonTypes>("up", out var up))
                    options.Up = up;
                if (gamepad.TryGetEnumValue<GamePadButtonTypes>("down", out var down))
                    options.Down = down;
                if (gamepad.TryGetEnumValue<GamePadButtonTypes>("left", out var left))
                    options.Left = left;
                if (gamepad.TryGetEnumValue<GamePadButtonTypes>("right", out var right))
                    options.Right = right;
                if (gamepad.TryGetEnumValue<GamePadButtonTypes>("rotate_left", out var rotateLeft))
                    options.RotateLeft = rotateLeft;
                if (gamepad.TryGetEnumValue<GamePadButtonTypes>("rotate_right", out var rotateRight))
                    options.RotateRight = rotateRight;
                if (gamepad.TryGetEnumValue<GamePadButtonTypes>("drop", out var drop))
                    options.Drop = drop;
                if (gamepad.TryGetEnumValue<GamePadButtonTypes>("swap", out var swap))
                    options.Swap = swap;
                if (gamepad.TryGetEnumValue<GamePadButtonTypes>("pause", out var pause))
                    options.Pause = pause;
            }

            return options;
        }
    }
}
