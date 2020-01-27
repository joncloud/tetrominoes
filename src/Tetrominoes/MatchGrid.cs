using Microsoft.Xna.Framework;
using System;

namespace Tetrominoes
{
    public class MatchGrid
    {
        public const int Width = 12;
        public const int Height = 32;
        public const int Size = Width * Height;
        readonly TetrominoPiece[] _data = new TetrominoPiece[Size];

        public TetrominoPiece this[int i]
        {
            get
            {
                if (i < 0) throw new ArgumentOutOfRangeException(nameof(i));
                if (i >= Size) throw new ArgumentOutOfRangeException(nameof(i));

                return _data[i];
            }
        }

        public TetrominoPiece this[int x, int y]
        {
            get => _data[GetIndex(x, y)];
            set => _data[GetIndex(x, y)] = value;
        }

        public TetrominoPiece this[Point p]
        {
            get => _data[GetIndex(p.X, p.Y)];
            set => _data[GetIndex(p.X, p.Y)] = value;
        }

        public TetrominoPiece this[Vector2 v]
        {
            get => _data[GetIndex((int)v.X, (int)v.Y)];
            set => _data[GetIndex((int)v.X, (int)v.Y)] = value;
        }

        public static int GetIndex(int x, int y)
        {
            var index = x + Width * y;
            if (index < 0 || index >= Size) throw new ArgumentOutOfRangeException(nameof(x));
            return index;
        }

        public static Point GetPosition(int index)
        {
            if (index < 0 || index >= Size) throw new ArgumentOutOfRangeException(nameof(index));

            var x = index % Width;
            var y = index / Width;
            return new Point(x, y);
        }
    }
}
