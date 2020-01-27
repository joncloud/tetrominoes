using Microsoft.Xna.Framework;
using System;

namespace Tetrominoes
{
    public class MatchTetrominoes : IDisposable
    {
        readonly Random _random;
        public Match Match { get; }
        Tetromino _current;
        public Tetromino Current => _current;
        public Tetromino Shadow { get; private set; }
        public Tetromino Next { get; private set; }
        Tetromino _swap;
        public Tetromino Swap => _swap;

        public MatchTetrominoes(Random random, Match match)
        {
            _random = random ?? throw new ArgumentNullException(nameof(random));
            Match = match ?? throw new ArgumentNullException(nameof(match));
        }

        Tetromino RandomTetromino()
        {
            var piece = (TetrominoPiece)_random.Next(1, 8);
            return new Tetromino(piece);
        }

        public void SwapPieces()
        {
            if (Swap == default)
            {
                _swap = Next;
                Next = RandomTetromino();
            }

            Utility.Swap(ref _swap, ref _current);
            Utility.Swap(ref _swap.Position, ref _current.Position);

            Shadow.Dispose();
            Shadow = new Tetromino(Current.Piece)
            {
                Position = Current.Position
            };
            ProjectShadow();

            var shouldSwapBack = false;
            for (var i = 0; i < _current.Squares.Length; i++)
            {
                var shouldBreak = false;
                while ((_current.Squares[i].X + _current.Position.X) < 0)
                {
                    shouldBreak = true;

                    var pos = _current.Position.X;
                    Match.Controller.Move.Right();
                    if (pos == _current.Position.X)
                    {
                        shouldSwapBack = true;
                        shouldBreak = true;
                        break;
                    }
                }

                if (shouldBreak) break;
                while (_current.Squares[i].X + _current.Position.X >= MatchGrid.Width)
                {
                    shouldBreak = true;

                    var pos = _current.Position.X;
                    Match.Controller.Move.Left();
                    if (pos == _current.Position.X)
                    {
                        shouldSwapBack = true;
                        shouldBreak = true;
                        break;
                    }
                }
                if (shouldBreak) break;
            }

            if (shouldSwapBack)
            {
                SwapPieces();
            }
        }

        public void NextPiece()
        {
            if (Shadow != null)
            {
                Shadow.Dispose();
            }

            if (Next == null)
            {
                _current = RandomTetromino();
            }
            else
            {
                _current.Dispose();
                _current = Next;
            }
            _current.Position = new Vector2(MatchGrid.Width / 2, 2);

            Shadow = new Tetromino(Current.Piece)
            {
                Position = Current.Position
            };
            ProjectShadow();

            Next = RandomTetromino();
        }

        public void ProjectShadow()
        {
            Array.Copy(
                Current.Squares,
                0,
                Shadow.Squares,
                0,
                Shadow.Squares.Length
            );
            Shadow.Position = Current.Position;
            var delta = new Vector2(0, 1);
            while (Match.Controller.Move.CheckMove(Shadow, delta) == TetrominoAction.Move)
            {
                Shadow.Position += delta;
            }
        }

        public void Dispose()
        {
            _current?.Dispose();
            Shadow?.Dispose();
            Next?.Dispose();
            Swap?.Dispose();
        }
    }
}
