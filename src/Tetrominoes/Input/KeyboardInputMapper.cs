using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using Tetrominoes.Options;

namespace Tetrominoes.Input
{
    public class KeyboardInputMapper : IInputMapper
    {
        readonly IOptionService _options;
        GameInputKeyboardOptions KeyboardOptions => _options.Options.Input.Keyboard;
        public InputState Current { get; private set; }
        public KeyboardInputMapper(IOptionService options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public static bool IsEnabled(IOptionService options) =>
            options.Options.Input.Keyboard.Enabled;

        static InputButton GetStateFor(in KeyboardState current, in Keys key, in InputButton button, GameTime gameTime)
        {
            return current.IsKeyDown(key)
                ? button.Press(gameTime)
                : button.Release();
        }

        public InputConnection Update(GameTime gameTime)
        {
            var keyboard = Keyboard.GetState();

            Current = new InputState
            {
                RotateLeft = GetStateFor(keyboard, KeyboardOptions.RotateLeft, Current.RotateLeft, gameTime),
                RotateRight = GetStateFor(keyboard, KeyboardOptions.RotateRight, Current.RotateRight, gameTime),
                Drop = GetStateFor(keyboard, KeyboardOptions.Drop, Current.Drop, gameTime),
                Swap = GetStateFor(keyboard, KeyboardOptions.Swap, Current.Swap, gameTime),
                Up = GetStateFor(keyboard, KeyboardOptions.Up, Current.Up, gameTime),
                Down = GetStateFor(keyboard, KeyboardOptions.Down, Current.Down, gameTime),
                Left = GetStateFor(keyboard, KeyboardOptions.Left, Current.Left, gameTime),
                Right = GetStateFor(keyboard, KeyboardOptions.Right, Current.Right, gameTime),
                Pause = GetStateFor(keyboard, KeyboardOptions.Pause, Current.Pause, gameTime),
                Back = GetStateFor(keyboard, KeyboardOptions.Back, Current.Back, gameTime),
            };

            return InputConnection.Connected;
        }

        public InputConnection GetConnectionState() =>
            InputConnection.Connected;

        public override string ToString() =>
            "Keyboard";
    }
}
