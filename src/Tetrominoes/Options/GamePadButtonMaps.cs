using Microsoft.Xna.Framework.Input;
using System;

namespace Tetrominoes.Options
{
    public static class GamePadButtonMaps
    {
        public static GetButtonState GetMapFor(GamePadButtonTypes type)
        {
            return type switch
            {
                GamePadButtonTypes.ButtonsA => Buttons.A,
                GamePadButtonTypes.ButtonsB => Buttons.B,
                GamePadButtonTypes.ButtonsX => Buttons.X,
                GamePadButtonTypes.ButtonsY => Buttons.Y,
                GamePadButtonTypes.ButtonsBack => Buttons.Back,
                GamePadButtonTypes.ButtonsRightShoulder => Buttons.RightShoulder,
                GamePadButtonTypes.ButtonsRightStick => Buttons.RightStick,
                GamePadButtonTypes.ButtonsLeftShoulder => Buttons.LeftShoulder,
                GamePadButtonTypes.ButtonsLeftStick => Buttons.LeftStick,
                GamePadButtonTypes.ButtonsStart => Buttons.Start,
                GamePadButtonTypes.ButtonsBigButton => Buttons.BigButton,
                GamePadButtonTypes.DPadUp => DPad.Up,
                GamePadButtonTypes.DPadDown => DPad.Down,
                GamePadButtonTypes.DPadLeft => DPad.Left,
                GamePadButtonTypes.DPadRight => DPad.Right,
                _ => throw new ArgumentOutOfRangeException(nameof(type))
            };
        }

        public static class DPad
        {
            public static ButtonState Up(in GamePadState state) => state.DPad.Up;
            public static ButtonState Down(in GamePadState state) => state.DPad.Down;
            public static ButtonState Left(in GamePadState state) => state.DPad.Left;
            public static ButtonState Right(in GamePadState state) => state.DPad.Right;
        }

        public static class Buttons
        {
            public static ButtonState RightShoulder(in GamePadState state) => state.Buttons.RightShoulder;
            public static ButtonState LeftStick(in GamePadState state) => state.Buttons.LeftStick;
            public static ButtonState LeftShoulder(in GamePadState state) => state.Buttons.LeftShoulder;
            public static ButtonState Start(in GamePadState state) => state.Buttons.Start;
            public static ButtonState Y(in GamePadState state) => state.Buttons.Y;
            public static ButtonState X(in GamePadState state) => state.Buttons.X;
            public static ButtonState RightStick(in GamePadState state) => state.Buttons.RightStick;
            public static ButtonState Back(in GamePadState state) => state.Buttons.Back;
            public static ButtonState A(in GamePadState state) => state.Buttons.A;
            public static ButtonState B(in GamePadState state) => state.Buttons.B;
            public static ButtonState BigButton(in GamePadState state) => state.Buttons.BigButton;
        }
    }
}
