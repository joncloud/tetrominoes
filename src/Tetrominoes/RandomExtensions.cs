using System;
using System.Collections.Generic;

namespace Tetrominoes
{
    public static class RandomExtensions
    {
        public static bool NextChance(this Random random, int percent)
        {
            var expected = percent / 100.0;
            var actual = random.NextDouble();
            return expected <= actual;
        }

        public static T NextEnum<T>(this Random random)
            where T : Enum
        {
            if (random == default) throw new ArgumentNullException(nameof(random));

            var values = Enum.GetValues(typeof(T));
            var index = random.Next(values.Length);
            var value = values.GetValue(index);
            if (value == null) throw new InvalidOperationException();
            return (T)value;
        }

        public static T NextElement<T>(this Random random, IReadOnlyList<T> array)
        {
            if (random == default) throw new ArgumentNullException(nameof(random));
            if (array == default) throw new ArgumentNullException(nameof(array));

            var index = random.Next(array.Count);
            return array[index];
        }
    }
}
