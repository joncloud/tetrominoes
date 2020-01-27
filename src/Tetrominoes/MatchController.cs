#nullable disable

using System;

namespace Tetrominoes
{
    public class MatchController
    {
        public Match Match { get; }
        public MatchMove Move { get; }
        public MatchRotate Rotate { get; }
        public MatchController(Match match)
        {
            Match = match ?? throw new ArgumentNullException(nameof(match));
            Move = new MatchMove(match);
            Rotate = new MatchRotate(match);
        }

        public void Swap()
        {
            Match.Score.AddSwap();

            Match.Tetrominoes.SwapPieces();
        }
    }
}
