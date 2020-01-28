using System;
using System.Collections.Generic;
using System.Linq;

namespace Tetrominoes.Options
{
    public class OptionItemList<T> : IOption
    {
        public string Name { get; }
        public IReadOnlyList<T> Values { get; }
        public T SelectedValue
        {
            get
            {
                var result = Values[SelectedIndex];
                return result ?? throw new InvalidOperationException("Unexpected null for option list");
            }
        }
        int _selectedIndex;
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                if (value < 0 || value >= Values.Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }
                _selectedIndex = value;
            }
        }
        public OptionItemList(string name, T selectedValue, IEnumerable<T> values)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            if (selectedValue == null) throw new ArgumentNullException(nameof(selectedValue));
            if (values == default) throw new ArgumentNullException(nameof(values));

            var list = values.ToList();
            if (!list.Contains(selectedValue)) list.Insert(0, selectedValue);
            Values = list.AsReadOnly();
            SelectedIndex = list.IndexOf(selectedValue);
        }

        public void Next(int offset)
        {
            var next = SelectedIndex + offset;
            if (next < 1) next = Values.Count - 1;
            if (next >= Values.Count) next = 0;
            SelectedIndex = next;
        }

        public override string ToString() => SelectedValue?.ToString() ?? "";
    }
}
