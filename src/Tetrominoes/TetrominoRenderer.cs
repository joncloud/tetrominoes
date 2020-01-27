#nullable disable

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Tetrominoes
{
    public class TetrominoRenderer
    {
        readonly Texture2D _tiles;
        readonly SpriteBatch _spriteBatch;
        public TetrominoRenderer(Texture2D tiles, SpriteBatch spriteBatch)
        {
            _tiles = tiles ?? throw new ArgumentNullException(nameof(tiles));
            _spriteBatch = spriteBatch ?? throw new ArgumentNullException(nameof(spriteBatch));
        }

        public void Draw(Tetromino tetromino, Color color)
        {
            for (var i = 0; i < tetromino.Squares.Length; i++)
            {
                var square = (tetromino.Squares[i] + tetromino.Position) * 8;
                _spriteBatch.Draw(
                    _tiles,
                    square,
                    new Rectangle(0, 0, 8, 8),
                    color
                );
            }
        }
    }
}
