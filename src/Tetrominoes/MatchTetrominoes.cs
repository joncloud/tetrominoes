using Microsoft.Xna.Framework;
using System;

namespace Tetrominoes
{
    public class MatchTetrominoes : IDisposable
    {
        readonly IMatchCalculator _calculator;
        public Match Match { get; }
        Tetromino _current;
        public Tetromino Current => _current;
        public Tetromino Shadow { get; private set; }
        public Tetromino Next { get; private set; }
        Tetromino? _swap;
        public Tetromino? Swap => _swap;

        public MatchTetrominoes(IMatchCalculator calculator, Match match)
        {
            _calculator = calculator ?? throw new ArgumentNullException(nameof(calculator));
            Match = match ?? throw new ArgumentNullException(nameof(match));

            _current = RandomTetromino();
            Center(_current);

            Next = RandomTetromino();

            Shadow = CreateShadow(Current);
            ProjectShadow();
        }

        Tetromino RandomTetromino()
        {
            var piece = _calculator.NextTetrominoPiece();
            return new Tetromino(piece);
        }

        public void SwapPieces()
        {
            if (Swap == default)
            {
                _swap = Next;
                Next = RandomTetromino();
            }

            // Swap isn't null here, because it was just
            // checked for, and if it is null it gets pulled
            // from Next which can't be null.
#pragma warning disable CS8601 // Possible null reference assignment.
            Utility.Swap(ref _swap, ref _current);
#pragma warning restore CS8601 // Possible null reference assignment.
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

        static void Center(Tetromino tetromino)
        {
            tetromino.Position = new Vector2(
                (MatchGrid.Width / 2) - 1, 
                2
            );
        }

        public void NextPiece()
        {
            Shadow.Dispose();
            _current.Dispose();

            _current = Next;
            Center(_current);

            Shadow = CreateShadow(Current);
            ProjectShadow();

            Next = RandomTetromino();
        }

        static Tetromino CreateShadow(Tetromino caster) =>
            new Tetromino(caster.Piece)
            {
                Position = caster.Position
            };

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
