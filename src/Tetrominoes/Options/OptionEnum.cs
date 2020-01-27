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

        public OptionEnum(string name, T selectedValue)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            _selectedValue = selectedValue ?? throw new ArgumentNullException(nameof(selectedValue));
        }
    }
}
