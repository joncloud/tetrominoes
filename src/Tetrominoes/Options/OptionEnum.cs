using System;

namespace Tetrominoes.Options
{
    public class OptionEnum<T> : IOption
        where T : Enum
    {
        public string Name { get; }
        T _selectedValue;
        public T SelectedValue
        {
            get => _selectedValue;
            set
            {
                _selectedValue = value ?? throw new ArgumentNullException(nameof(value));
            }
        }

        readonly Func<T, string> _toString;
        public OptionEnum(string name, T selectedValue)
            : this(name, selectedValue, x => x.ToString())
        {
        }

        public OptionEnum(string name, T selectedValue, Func<T, string> toString)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            _selectedValue = selectedValue ?? throw new ArgumentNullException(nameof(selectedValue));
            _toString = toString ?? throw new ArgumentNullException(nameof(toString));
        }

        public override string ToString() => _toString(_selectedValue) ?? "";
    }
}
