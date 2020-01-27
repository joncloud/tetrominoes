using Microsoft.Xna.Framework;
using System;

namespace Tetrominoes
{
    public class Tetromino : IDisposable
    {
        static class Pieces
        {
            public static readonly Vector2[] Straight = new[]
            {
                new Vector2(-1, 0),
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(2, 0)
            };
            public static readonly Vector2[] T = new[]
            {
                new Vector2(-1, 0),
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(1, 0)
            };
            public static readonly Vector2[] Square = new[]
            {
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(1, 1),
                new Vector2(0, 1)
            };
            public static readonly Vector2[] J = new[]
            {
                new Vector2(2, 0),
                new Vector2(1, 0),
                new Vector2(0, 0),
                new Vector2(0, 1)
            };
            public static readonly Vector2[] L = new[]
            {
                new Vector2(0, 2),
                new Vector2(0, 1),
                new Vector2(0, 0),
                new Vector2(1, 0)
            };
            public static readonly Vector2[] S = new[]
            {
                new Vector2(-1, 1),
                new Vector2(0, 1),
                new Vector2(0, 0),
                new Vector2(1, 0)
            };
            public static readonly Vector2[] Z = new[]
            {
                new Vector2(-1, 0),
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(1, 1)
            };
        }

        public static readonly ObjectPool<Vector2[]> SquarePool = new ObjectPool<Vector2[]>(
            () => new Vector2[4]
        );
        public Tetromino(TetrominoPiece piece)
        {
            var source = piece switch
            {
                TetrominoPiece.J => Pieces.J,
                TetrominoPiece.L => Pieces.L,
                TetrominoPiece.S => Pieces.S,
                TetrominoPiece.Square => Pieces.Square,
                TetrominoPiece.Straight => Pieces.Straight,
                TetrominoPiece.T => Pieces.T,
                TetrominoPiece.Z => Pieces.Z,
                _ => throw new ArgumentNullException(nameof(piece))
            };
            Piece = piece;
            Squares = SquarePool.Rent();
            Array.Copy(source, 0, Squares, 0, source.Length);
        }

        public TetrominoPiece Piece { get; }
        public Vector2[] Squares;
        public Vector2 Position;

        bool _disposed;
        public void Dispose()
        {
            if (_disposed) return;
            SquarePool.Return(Squares);
            _disposed = true;
        }
    }
}
