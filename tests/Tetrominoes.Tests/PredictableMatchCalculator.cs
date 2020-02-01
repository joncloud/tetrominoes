using System;
using System.Collections.Generic;

namespace Tetrominoes.Tests
{
    class PredictableMatchCalculator : IMatchCalculator
    {
        readonly Queue<TetrominoPiece> _queue;
        public PredictableMatchCalculator(params TetrominoPiece[] pieces) =>
            _queue = new Queue<TetrominoPiece>(pieces ?? throw new ArgumentNullException(nameof(pieces)));
        public TetrominoPiece NextTetrominoPiece() =>
            _queue.Dequeue();
    }
}
