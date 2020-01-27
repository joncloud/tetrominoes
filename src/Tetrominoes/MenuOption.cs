#nullable disable

using Microsoft.Xna.Framework;
using System;

namespace Tetrominoes
{
    public class MenuOption
    {
        readonly Action _fn;
        public string Text { get; }
        public Vector2 Position { get; }
        public MenuOption(Action fn, string text, Vector2 position)
        {
            _fn = fn ?? throw new ArgumentNullException(nameof(fn));
            Text = text ?? throw new ArgumentNullException(nameof(text));
            Position = position;
        }
        public void Choose() =>
            _fn();
    }
}
