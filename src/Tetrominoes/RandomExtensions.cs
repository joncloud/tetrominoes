using System;
using System.Collections.Generic;

namespace Tetrominoes
{
    public static class RandomExtensions
    {
        public static T NextElement<T>(this Random random, IReadOnlyList<T> array)
        {
            if (random == default) throw new ArgumentNullException(nameof(random));
            if (array == default) throw new ArgumentNullException(nameof(array));

            var index = random.Next(array.Count);
            return array[index];
        }
    }
}
