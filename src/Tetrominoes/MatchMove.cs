using Microsoft.Xna.Framework;
using System;

namespace Tetrominoes
{
    public class MatchMove
    {
        public Match Match { get; }
        public MatchMove(Match match)
        {
            Match = match ?? throw new ArgumentNullException(nameof(match));
        }

        public TetrominoAction CheckMove(Tetromino tetromino, Vector2 delta)
        {
            for (var i = 0; i < tetromino.Squares.Length; i++)
            {
                var square = tetromino.Squares[i] + tetromino.Position + delta;
                if (square.X < 0)
                {
                    return TetrominoAction.None;
                }
                else if (square.X == MatchGrid.Width)
                {
                    return TetrominoAction.None;
                }

                if (square.Y < 0)
                {
                    return TetrominoAction.None;
                }
                else if (square.Y == MatchGrid.Height)
                {
                    return TetrominoAction.Lock;
                }

                var tile = Match.Grid[square];
                if (tile != TetrominoPiece.Empty)
                {
                    return TetrominoAction.Lock;
                }
            }
            return TetrominoAction.Move;
        }

        TetrominoAction Move(Tetromino tetromino, Vector2 delta, bool canLock, MovementType type)
        {
            Match.Score.AddMovement(type);

            var action = CheckMove(tetromino, delta);
            switch (action)
            {
                case TetrominoAction.Move:
                    tetromino.Position += delta;
                    Match.Tetrominoes.ProjectShadow();
                    break;

                case TetrominoAction.Lock:
                    if (!canLock)
                    {
                        return TetrominoAction.None;
                    }

                    Span<int> check = stackalloc int[tetromino.Squares.Length];
                    for (var i = 0; i < tetromino.Squares.Length; i++)
                    {
                        var square = tetromino.Squares[i] + tetromino.Position;
                        Match.Grid[square] = tetromino.Piece;

                        check[i] = (int)square.Y;
                    }

                    SelectionSort(check);

                    Match.Score.AddPieceLocked(tetromino.Piece);

                    var cleared = 0;
                    for (var i = 0; i < check.Length; i++)
                    {
                        if (i > 0 && check[i - 1] == check[i])
                        {
                            continue;
                        }

                        if (CheckRow(check[i]))
                        {
                            cleared++;
                        }
                    }

                    if (cleared > 0)
                    {
                        Match.Score.AddRowsCleared(cleared);
                    }
                    Match.Tetrominoes.NextPiece();
                    break;
            }

            return action;
        }

        /// <remarks>
        /// https://en.wikipedia.org/wiki/Selection_sort
        /// </remarks>
        static void SelectionSort(Span<int> a)
        {
            /* a[0] to a[aLength-1] is the array to sort */
            int i, j;
            int aLength = a.Length; // initialise to a's length

            /* advance the position through the entire array */
            /*   (could do i < aLength-1 because single element is also min element) */
            for (i = 0; i < aLength - 1; i++)
            {
                /* find the min element in the unsorted a[i .. aLength-1] */
                /* assume the min is the first element */
                int jMin = i;

                /* test against elements after i to find the smallest */
                for (j = i + 1; j < aLength; j++)
                {
                    /* if this element is less, then it is the new minimum */
                    if (a[j] < a[jMin])
                    {
                        /* found new minimum; remember its index */
                        jMin = j;
                    }
                }

                if (jMin != i)
                {
                    Utility.Swap(ref a[i], ref a[jMin]);
                }
            }
        }

        bool CheckRow(int y)
        {
            for (var x = 0; x < MatchGrid.Width; x++)
            {
                var piece = Match.Grid[x, y];
                if (piece == TetrominoPiece.Empty) return false;
            }

            for (var x = 0; x < MatchGrid.Width; x++)
            {
                for (var col = y; col >= 0; col--)
                {
                    var piece = col == 0
                        ? TetrominoPiece.Empty
                        : Match.Grid[x, col - 1];
                    Match.Grid[x, col] = piece;
                }
            }
            return true;
        }

        public void Left()
        {
            var delta = new Vector2(-1, 0);
            Move(Match.Tetrominoes.Current, delta, canLock: false, type: MovementType.Manual);
        }
        public void Right()
        {
            var delta = new Vector2(1, 0);
            Move(Match.Tetrominoes.Current, delta, canLock: false, type: MovementType.Manual);
        }
        public void Down()
        {
            var delta = new Vector2(0, 1);
            Move(Match.Tetrominoes.Current, delta, canLock: true, type: MovementType.Automatic);
        }
    }
}
