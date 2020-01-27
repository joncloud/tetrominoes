using Microsoft.Xna.Framework.Input;
using System;
using Tetrominoes.Options;

namespace Tetrominoes.Input
{
    public class KeyboardInputMapper : IInputMapper
    {
        readonly IOptionService _options;
        GameInputKeyboardOptions KeyboardOptions => _options.Options.Input.Keyboard;
        KeyboardState _last;
        public InputState Current { get; private set; }
        public KeyboardInputMapper(IOptionService options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public static bool IsEnabled(IOptionService options) =>
            options.Options.Input.Keyboard.Enabled;

        static InputButtonState GetStateFor(in KeyboardState last, in KeyboardState current, in Keys key)
        {
            var wasDown = last.IsKeyDown(key);
            var isDown = current.IsKeyDown(key);

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
            var current = Keyboard.GetState();

            Current = new InputState
            {
                RotateLeft = GetStateFor(_last, current, KeyboardOptions.RotateLeft),
                RotateRight = GetStateFor(_last, current, KeyboardOptions.RotateRight),
                Drop = GetStateFor(_last, current, KeyboardOptions.Drop),
                Swap = GetStateFor(_last, current, KeyboardOptions.Swap),
                Up = GetStateFor(_last, current, KeyboardOptions.Up),
                Down = GetStateFor(_last, current, KeyboardOptions.Down),
                Left = GetStateFor(_last, current, KeyboardOptions.Left),
                Right = GetStateFor(_last, current, KeyboardOptions.Right),
                Pause = GetStateFor(_last, current, KeyboardOptions.Pause)
            };

            _last = current;
            return InputConnection.Connected;
        }

        public InputConnection GetConnectionState() =>
            InputConnection.Connected;

        public override string ToString() =>
            "KEYBOARD";
    }
}
