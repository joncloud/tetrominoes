using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using Tetrominoes.Options;

namespace Tetrominoes.Input
{
    public class GamePadInputMapper : IInputMapper
    {
        readonly IOptionService _options;
        GameInputGamePadOptions GamePadOptions => _options.Options.Input.GamePad;
        public InputState Current { get; private set; }
        public GamePadInputMapper(IOptionService options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public static bool IsEnabled(IOptionService options) =>
            options.Options.Input.GamePad.Enabled;

        static InputButton GetStateFor(in GamePadState current, GetButtonState get, in InputButton button, GameTime gameTime)
        {
            return get(current) == ButtonState.Pressed
                ? button.Press(gameTime)
                : button.Release();
        }

        public InputConnection Update(GameTime gameTime)
        {
            var gamePad = GamePad.GetState(
                GamePadOptions.PlayerIndex
            );

            Current = new InputState
            {
                RotateLeft = GetStateFor(gamePad, GamePadOptions.RotateLeftMap, Current.RotateLeft, gameTime),
                RotateRight = GetStateFor(gamePad, GamePadOptions.RotateRightMap, Current.RotateRight, gameTime),
                Drop = GetStateFor(gamePad, GamePadOptions.DropMap, Current.Drop, gameTime),
                Swap = GetStateFor(gamePad, GamePadOptions.SwapMap, Current.Swap, gameTime),
                Up = GetStateFor(gamePad, GamePadOptions.UpMap, Current.Up, gameTime),
                Down = GetStateFor(gamePad, GamePadOptions.DownMap, Current.Down, gameTime),
                Left = GetStateFor(gamePad, GamePadOptions.LeftMap, Current.Left, gameTime),
                Right = GetStateFor(gamePad, GamePadOptions.RightMap, Current.Right, gameTime),
                Pause = GetStateFor(gamePad, GamePadOptions.PauseMap, Current.Pause, gameTime),
                Back = GetStateFor(gamePad, GamePadOptions.BackMap, Current.Back, gameTime),
            };

            return gamePad.IsConnected 
                ? InputConnection.Connected 
                : InputConnection.Disconnected;
        }

        public InputConnection GetConnectionState()
        {
            var current = GamePad.GetState(
                GamePadOptions.PlayerIndex
            );
            return current.IsConnected 
                ? InputConnection.Connected
                : InputConnection.Disconnected;
        }

        public override string ToString() =>
            $"CONTROLLER {GamePadOptions.PlayerIndex.ToString().ToUpper()}";
    }
}
