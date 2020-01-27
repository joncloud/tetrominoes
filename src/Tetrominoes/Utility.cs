#nullable disable

namespace Tetrominoes
{
    public static class Utility
    {
        public static void Swap<T>(ref T x, ref T y)
        {
            var tmp = x;
            x = y;
            y = tmp;
        }
    }
}
