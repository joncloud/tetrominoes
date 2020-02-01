using Microsoft.Xna.Framework;
using System;
using Tetrominoes.Options;

namespace Tetrominoes
{
    public class Match : IDisposable
    {
        public MatchGrid Grid { get; }
        public MatchTetrominoes Tetrominoes { get; }
        public MatchController Controller { get; }
        public MatchScore Score { get; }
        public Match(Random random, IOptionService option)
            : this(
                  new RandomMatchCalculator(random),
                  option
            )
        {
        }

        public Match(IMatchCalculator calculator, IOptionService option)
        {
            if (calculator == default) throw new ArgumentNullException(nameof(calculator));
            if (option == default) throw new ArgumentNullException(nameof(option));

            Grid = new MatchGrid();
            Controller = new MatchController(this, option);
            Score = new MatchScore(this);
            Tetrominoes = new MatchTetrominoes(calculator, this);
        }

        TimeSpan _drop;
        public MatchState Update(GameTime gameTime)
        {
            Score.AddTime(gameTime.ElapsedGameTime);

            _drop += gameTime.ElapsedGameTime;
            while (_drop.TotalMilliseconds >= Score.Speed)
            {
                _drop -= TimeSpan.FromMilliseconds(Score.Speed);
                Controller.Move.Down();
            }

            for (var x = 0; x < MatchGrid.Width; x++)
            {
                var p = Grid[x, 2];
                if (p != TetrominoPiece.Empty)
                {
                    return MatchState.Lost;
                }
            }

            return MatchState.Playing;
        }

        public void Dispose()
        {
            Tetrominoes.Dispose();
        }
    }
}
