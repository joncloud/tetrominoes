using System;
using Tetrominoes.Options;

namespace Tetrominoes
{
    public class MatchController
    {
        readonly IOptionService _option;
        public Match Match { get; }
        public MatchMove Move { get; }
        public MatchRotate Rotate { get; }
        public MatchController(Match match, IOptionService option)
        {
            _option = option ?? throw new ArgumentNullException(nameof(option));

            Match = match ?? throw new ArgumentNullException(nameof(match));
            Move = new MatchMove(match);
            Rotate = new MatchRotate(match);
        }

        public void Swap()
        {
            if (!_option.Options.Gameplay.SwapPiece) return;

            Match.Score.AddSwap();

            Match.Tetrominoes.SwapPieces();
        }
    }
}
