using System;
using System.Collections.Generic;

namespace Tetrominoes.Tests
{
    class PredictableRandom : Random
    {
        readonly Queue<double> _samples;
        public PredictableRandom(params double[] samples)
            : this(new Queue<double>(samples ?? throw new ArgumentNullException(nameof(samples))))
        {
        }

        public PredictableRandom(Queue<double> samples)
        {
            _samples = samples ?? throw new ArgumentNullException(nameof(samples));
        }

        protected override double Sample()
        {
            if (_samples.Count > 0)
            {
                return _samples.Dequeue();
            }
            else
            {
                throw new InvalidOperationException("No samples remaining");
            }
        }
    }
}
