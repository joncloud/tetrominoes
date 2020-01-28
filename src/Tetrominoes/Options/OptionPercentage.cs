using Microsoft.Xna.Framework;
using System;

namespace Tetrominoes.Options
{
    public class OptionPercentage : IOption
    {
        public string Name { get; }
        int _selectedValue;
        public int SelectedValue
        {
            get => _selectedValue;
            set => _selectedValue = MathHelper.Clamp(value, 0, 100);
        }
        public OptionPercentage(string name, int selectedValue)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            SelectedValue = selectedValue;
        }

        public override string ToString() => (SelectedValue / 100.0f).ToString("P");
    }
}
