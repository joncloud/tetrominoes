using System;

namespace Tetrominoes.Options
{
    public class OptionToggle : IOption
    {
        public string Name { get; }
        public bool SelectedValue { get; set; }
        public OptionToggle(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
    }
}
