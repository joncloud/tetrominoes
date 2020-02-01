namespace Tetrominoes.Input
{
    public struct InputState : IInput<InputButton>
    {
        public InputButton Up { get; set; }
        public InputButton Down { get; set; }
        public InputButton Left { get; set; }
        public InputButton Right { get; set; }
        public InputButton RotateLeft { get; set; }
        public InputButton RotateRight { get; set; }
        public InputButton Drop { get; set; }
        public InputButton Swap { get; set; }
        public InputButton Pause { get; set; }
        public InputButton Back { get; set; }

        public static InputState operator +(InputState x, InputState y) =>
            new InputState
            {
                RotateLeft = x.RotateLeft + y.RotateLeft,
                RotateRight = x.RotateRight + y.RotateRight,
                Drop = x.Drop + y.Drop,
                Swap = x.Swap + y.Swap,
                Up = x.Up + y.Up,
                Down = x.Down + y.Down,
                Left = x.Left + y.Left,
                Right = x.Right + y.Right,
                Pause = x.Pause + y.Pause,
                Back = x.Back + y.Back
            };
    }
}
