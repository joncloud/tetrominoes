using Microsoft.Xna.Framework;
using System;

namespace Tetrominoes.Input
{
    public readonly struct InputButton
    {
        public InputButtonState State { get; }
        public TimeSpan Held { get; }
        public InputButton(InputButtonState state, TimeSpan held)
        {
            State = state;
            Held = held;
        }

        public bool IsHeldForInterval(int intervalMs) =>
            State == InputButtonState.Held &&
            ((int)Held.TotalMilliseconds) % intervalMs == 0;

        public InputButton Release() =>
            new InputButton(InputButtonState.Released, TimeSpan.Zero);

        public InputButton Press(GameTime gameTime) =>
            State switch
            {
                InputButtonState.Held => new InputButton(State, Held + gameTime.ElapsedGameTime),
                InputButtonState.Pressed => new InputButton(InputButtonState.Held, Held + gameTime.ElapsedGameTime),
                InputButtonState.Released => new InputButton(InputButtonState.Pressed, TimeSpan.Zero),
                _ => new InputButton(InputButtonState.Pressed, TimeSpan.Zero)
            };

        public static InputButton operator +(InputButton x, InputButton y) =>
            new InputButton(
                InputMath.GreaterOf(x.State, y.State),
                x.Held + y.Held
            );
    }
}
