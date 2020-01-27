using System;
using System.Collections.Generic;
using System.Linq;

namespace Tetrominoes.Options
{
    public class OptionItemList<T> : IOption
    {
        public string Name { get; }
        public IReadOnlyList<T> Values { get; }
        T _selectedValue;
        public T SelectedValue
        {
            get => _selectedValue;
            set
            {
                var v = value ?? throw new ArgumentNullException(nameof(value));
                if (!Values.Contains(v))
                {
                    throw new ArgumentException(nameof(value));
                }
                _selectedValue = v;
            }
        }
        public OptionItemList(string name, T selectedValue, IEnumerable<T> values)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            _selectedValue = selectedValue ?? throw new ArgumentNullException(nameof(selectedValue));
            if (values == default) throw new ArgumentNullException(nameof(values));

            var list = values.ToList();
            if (!list.Contains(_selectedValue)) list.Insert(0, _selectedValue);
            Values = list.AsReadOnly();
        }
    }
}
