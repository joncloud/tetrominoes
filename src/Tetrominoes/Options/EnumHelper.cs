using System;

namespace Tetrominoes.Options
{
    public static class EnumHelper<T>
        where T : Enum
    {
        public static T IfDefined(T current, T @default)
        {
            return Enum.IsDefined(typeof(T), current)
                ? current
                : @default;
        }
    }
}
