#nullable disable

using Microsoft.Xna.Framework;
using System;

namespace Tetrominoes
{
    public class MatchRotate
    {
        public Match Match { get; }
        public MatchRotate(Match match)
        {
            Match = match ?? throw new ArgumentNullException(nameof(match));
        }

        static void Round(Vector2[] array)
        {
            for (var i = 0; i < array.Length; i++)
            {
                var v = array[i];
                v.X = (int)Math.Round(v.X);
                v.Y = (int)Math.Round(v.Y);
                array[i] = v;
            }
        }

        void Rotate(Tetromino tetromino, RotationDirection direction, ref Matrix rotation)
        {
            Match.Score.AddRotation(direction);

            var target = Tetromino.SquarePool.Rent();
            Vector2.Transform(tetromino.Squares, ref rotation, target);
            
            // Round off the points to make sure that we're dealing with points
            // like 0 and 1, instead of something very close like 0.999999998.
            Round(target);

            var min = new Vector2();
            var max = new Vector2();
            for (var i = 0; i < target.Length; i++)
            {
                var square = target[i] + tetromino.Position;

                min.X = Math.Min(min.X, square.X);
                min.Y = Math.Min(min.Y, square.Y);
                max.X = Math.Max(max.X, square.X);
                max.Y = Math.Max(max.Y, square.Y);
            }

            var offset = new Vector2();
            if (min.X < 0)
            {
                offset.X = -min.X;
            }
            else if (max.X >= MatchGrid.Width - 1)
            {
                offset.X = MatchGrid.Width - 1 - max.X;
            }
            if (min.Y < 0)
            {
                offset.Y = -min.Y;
            }
            else if (max.Y >= MatchGrid.Height - 1)
            {
                offset.Y = MatchGrid.Height - 1 - max.Y;
            }

            for (var i = 0; i < target.Length; i++)
            {
                var square = target[i] + tetromino.Position + offset;
                if (Match.Grid[square] != TetrominoPiece.Empty)
                {
                    Tetromino.SquarePool.Return(target);
                    return;
                }
            }

            Tetromino.SquarePool.Return(tetromino.Squares);
            tetromino.Squares = target;
            tetromino.Position += offset;
            Match.Tetrominoes.ProjectShadow();
        }


        static Matrix _rotateLeft = Matrix.CreateRotationZ(
            MathHelper.PiOver2
        );
        public void Left()
        {
            Rotate(
                Match.Tetrominoes.Current,
                RotationDirection.Left,
                ref _rotateLeft
            );
        }


        static Matrix _rotateRight = Matrix.CreateRotationZ(
            -MathHelper.PiOver2
        );
        public void Right()
        {
            Rotate(
                Match.Tetrominoes.Current,
                RotationDirection.Right,
                ref _rotateRight
            );
        }
    }
}
