#nullable disable

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using Tetrominoes.Options;

namespace Tetrominoes.Input
{
    public class GamePadInputMapper : IInputMapper
    {
        readonly IOptionService _options;
        GamePadState _last;
        GameInputGamePadOptions GamePadOptions => _options.Options.Input.GamePad;
        public InputState Current { get; private set; }
        public GamePadInputMapper(IOptionService options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public static bool IsEnabled(IOptionService options) =>
            options.Options.Input.GamePad.Enabled;

        static InputButtonState GetStateFor(in GamePadState last, in GamePadState current, GetButtonState get)
        {
            var wasDown = get(last) == ButtonState.Pressed;
            var isDown = get(current) == ButtonState.Pressed;

            if (wasDown && isDown)
            {
                return InputButtonState.Held;
            }
            else if (wasDown)
            {
                return InputButtonState.Pressed;
            }
            else
            {
                return InputButtonState.Released;
            }
        }

        public InputConnection Update()
        {
            var current = GamePad.GetState(
                GamePadOptions.PlayerIndex
            );

            Current = new InputState
            {
                RotateLeft = GetStateFor(_last, current, GamePadOptions.RotateLeftMap),
                RotateRight = GetStateFor(_last, current, GamePadOptions.RotateRightMap),
                Drop = GetStateFor(_last, current, GamePadOptions.DropMap),
                Swap = GetStateFor(_last, current, GamePadOptions.SwapMap),
                Up = GetStateFor(_last, current, GamePadOptions.UpMap),
                Down = GetStateFor(_last, current, GamePadOptions.DownMap),
                Left = GetStateFor(_last, current, GamePadOptions.LeftMap),
                Right = GetStateFor(_last, current, GamePadOptions.RightMap),
                Pause = GetStateFor(_last, current, GamePadOptions.PauseMap),
            };

            _last = current;
            return current.IsConnected 
                ? InputConnection.Connected 
                : InputConnection.Disconnected;
        }

        public InputConnection GetConnectionState()
        {
            var current = GamePad.GetState(
                GamePadOptions.PlayerIndex
            );
            _last = current;
            return current.IsConnected 
                ? InputConnection.Connected
                : InputConnection.Disconnected;
        }

        public override string ToString() =>
            $"CONTROLLER {GamePadOptions.PlayerIndex.ToString().ToUpper()}";
    }
}
