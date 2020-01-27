using Nett;
using System;

namespace Tetrominoes.Options
{
    public static class TomlTableExtensions
    {
        public static bool TryGetEnumValue<T>(this TomlTable table, string key, out T value)
            where T : Enum
        {
            if (table == default) throw new ArgumentNullException(nameof(table));
            if (key == default) throw new ArgumentNullException(nameof(key));

            if (table.TryGetValue<T>(key, out var o) &&
                Enum.IsDefined(typeof(T), o))
            {
                value = o;
                return true;
            }

            // Default in this case will likely be 0, default of the enum value.
#pragma warning disable CS8653 // A default expression introduces a null value for a type parameter.
            value = default;
#pragma warning restore CS8653 // A default expression introduces a null value for a type parameter.
            return false;
        }

        public static bool TryGetTable(this TomlTable table, string key, out TomlTable value)
        {
            if (table == default) throw new ArgumentNullException(nameof(table));
            if (key == default) throw new ArgumentNullException(nameof(key));

            if (table.TryGetValue(key, out var o) && o is TomlTable t)
            {
                value = t;
                return true;
            }
            else
            {
                // Safety andled through if checks on Try pattern.
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                value = default;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                return false;
            }
        }

        public static bool TryGetValue<T>(this TomlTable table, string key, out T value)
        {
            if (table == default) throw new ArgumentNullException(nameof(table));
            if (key == default) throw new ArgumentNullException(nameof(key));

            if (table.TryGetValue(key, out var o))
            {
                value = o.Get<T>();
                return true;
            }
            else
            {
                // Safety andled through if checks on Try pattern.
#pragma warning disable CS8653 // A default expression introduces a null value for a type parameter.
                value = default;
#pragma warning restore CS8653 // A default expression introduces a null value for a type parameter.
                return false;
            }
        }
    }
}
