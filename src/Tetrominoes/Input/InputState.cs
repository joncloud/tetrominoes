#nullable disable

using Microsoft.Xna.Framework;

namespace Tetrominoes.Input
{
    public struct InputState
    {
        public InputButtonState Up { get; set; }
        public InputButtonState Down { get; set; }
        public InputButtonState Left { get; set; }
        public InputButtonState Right { get; set; }
        public InputButtonState RotateLeft { get; set; }
        public InputButtonState RotateRight { get; set; }
        public InputButtonState Drop { get; set; }
        public InputButtonState Swap { get; set; }
        public InputButtonState Pause { get; set; }

        public static InputState operator +(InputState x, InputState y) =>
            new InputState
            {
                RotateLeft = InputMath.GreaterOf(x.RotateLeft, y.RotateLeft),
                RotateRight = InputMath.GreaterOf(x.RotateRight, y.RotateRight),
                Drop = InputMath.GreaterOf(x.Drop, y.Drop),
                Swap = InputMath.GreaterOf(x.Swap, y.Swap),
                Up = InputMath.GreaterOf(x.Up, y.Up),
                Down = InputMath.GreaterOf(x.Down, y.Down),
                Left = InputMath.GreaterOf(x.Left, y.Left),
                Right = InputMath.GreaterOf(x.Right, y.Right),
                Pause = InputMath.GreaterOf(x.Pause, y.Pause)
            };
    }
}
