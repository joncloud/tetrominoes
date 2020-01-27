using System;

namespace Tetrominoes.Input
{
    public static class InputMath
    {
        public static InputButtonState GreaterOf(InputButtonState x, InputButtonState y) =>
            (InputButtonState)Math.Max((int)x, (int)y);
    }
}
