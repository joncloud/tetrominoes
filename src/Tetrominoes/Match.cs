using Microsoft.Xna.Framework;
using System;

namespace Tetrominoes
{
    public class Match : IDisposable
    {
        public MatchGrid Grid { get; }
        public MatchTetrominoes Tetrominoes { get; }
        public MatchController Controller { get; }
        public MatchScore Score { get; }
        public Random Random { get; }
        public Match(Random random)
        {
            Random = random ?? throw new ArgumentNullException(nameof(random));
            Controller = new MatchController(this);
            Tetrominoes = new MatchTetrominoes(random, this);
            Score = new MatchScore(this);
            Grid = new MatchGrid();
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
