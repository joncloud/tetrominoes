using System;

namespace Tetrominoes.Options
{
    public class OptionHeader : IOption
    {
        public string Name { get; }
        public OptionHeader(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
        public override string ToString() => "";
    }
}
