#nullable disable

using Microsoft.Xna.Framework;
using Nett;

namespace Tetrominoes.Options
{
    public class GameInputGamePadOptions
    {
        public static GameInputGamePadOptions CreateDefault() =>
            new GameInputGamePadOptions
            {
                Enabled = true,
                PlayerIndex = PlayerIndex.One,
                Up = GamePadButtonTypes.DPadUp,
                Down = GamePadButtonTypes.DPadDown,
                Left = GamePadButtonTypes.DPadLeft,
                Right = GamePadButtonTypes.DPadRight,
                RotateLeft = GamePadButtonTypes.ButtonsA,
                RotateRight = GamePadButtonTypes.ButtonsB,
                Drop = GamePadButtonTypes.ButtonsLeftShoulder,
                Swap = GamePadButtonTypes.ButtonsRightShoulder,
                Pause = GamePadButtonTypes.ButtonsStart
            };

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

        internal void ValidateConstraints()
        {
            PlayerIndex = EnumHelper<PlayerIndex>.IfDefined(PlayerIndex, PlayerIndex.One);
            Up = EnumHelper<GamePadButtonTypes>.IfDefined(Up, GamePadButtonTypes.DPadUp);
            Down = EnumHelper<GamePadButtonTypes>.IfDefined(Down, GamePadButtonTypes.DPadDown);
            Left = EnumHelper<GamePadButtonTypes>.IfDefined(Left, GamePadButtonTypes.DPadLeft);
            Right = EnumHelper<GamePadButtonTypes>.IfDefined(Right, GamePadButtonTypes.DPadRight);
            RotateLeft = EnumHelper<GamePadButtonTypes>.IfDefined(RotateLeft, GamePadButtonTypes.ButtonsA);
            RotateRight = EnumHelper<GamePadButtonTypes>.IfDefined(RotateRight, GamePadButtonTypes.ButtonsB);
            Drop = EnumHelper<GamePadButtonTypes>.IfDefined(Drop, GamePadButtonTypes.ButtonsLeftShoulder);
            Swap = EnumHelper<GamePadButtonTypes>.IfDefined(Swap, GamePadButtonTypes.ButtonsRightShoulder);
            Pause = EnumHelper<GamePadButtonTypes>.IfDefined(Pause, GamePadButtonTypes.ButtonsStart);
        }
    }
}
