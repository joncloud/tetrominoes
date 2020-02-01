using System;

namespace Tetrominoes
{
    public class RandomMatchCalculator : IMatchCalculator
    {
        readonly Random _random;
        public RandomMatchCalculator(Random random) =>
            _random = random ?? throw new ArgumentNullException(nameof(random));

        public TetrominoPiece NextTetrominoPiece() =>
            (TetrominoPiece)_random.Next(1, 8);
    }
}
