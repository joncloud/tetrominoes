using Microsoft.Xna.Framework;
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
                GamePadButtonTypes.TriggersLeft => Triggers.Left,
                GamePadButtonTypes.TriggersRight => Triggers.Right,
                GamePadButtonTypes.ThumbSticksLeftUp => ThumbSticks.Left.Up,
                GamePadButtonTypes.ThumbSticksLeftDown => ThumbSticks.Left.Down,
                GamePadButtonTypes.ThumbSticksLeftLeft => ThumbSticks.Left.Left,
                GamePadButtonTypes.ThumbSticksLeftRight => ThumbSticks.Left.Right,
                GamePadButtonTypes.ThumbSticksRightUp => ThumbSticks.Right.Up,
                GamePadButtonTypes.ThumbSticksRightDown => ThumbSticks.Right.Down,
                GamePadButtonTypes.ThumbSticksRightLeft => ThumbSticks.Right.Left,
                GamePadButtonTypes.ThumbSticksRightRight => ThumbSticks.Right.Right,
                _ => throw new ArgumentOutOfRangeException(nameof(type))
            };
        }

        public class ThumbStick
        {
            readonly Func<GamePadState, Vector2> _fn;
            public ThumbStick(Func<GamePadState, Vector2> fn) =>
                _fn = fn ?? throw new ArgumentNullException(nameof(fn));

            public ButtonState Up(in GamePadState state) => _fn(state).Y < -0.1f ? ButtonState.Pressed : ButtonState.Released;
            public ButtonState Down(in GamePadState state) => _fn(state).Y > 0.1f ? ButtonState.Pressed : ButtonState.Released;
            public ButtonState Left(in GamePadState state) => _fn(state).X < -0.1f ? ButtonState.Pressed : ButtonState.Released;
            public ButtonState Right(in GamePadState state) => _fn(state).X > 0.1f ? ButtonState.Pressed : ButtonState.Released;
        }

        public static class ThumbSticks
        {
            public static ThumbStick Left { get; } = new ThumbStick(s => s.ThumbSticks.Left);
            public static ThumbStick Right { get; } = new ThumbStick(s => s.ThumbSticks.Right);
        }

        public static class Triggers
        {
            public static ButtonState Left(in GamePadState state) => state.Triggers.Left > 0.1f ? ButtonState.Pressed : ButtonState.Released;
            public static ButtonState Right(in GamePadState state) => state.Triggers.Right > 0.1f ? ButtonState.Pressed : ButtonState.Released;
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
