#nullable disable

using System;
using Tetrominoes.Options;

namespace Tetrominoes.Input
{
    public static class InputMapperExtensions
    {
        public static IInputMapper HandleEnabledChange(this IInputMapper mapper, IOptionService options, Func<IOptionService, bool> enabled)
        {
            if (mapper == default) throw new ArgumentNullException(nameof(mapper));

            return new EnabledWrapperInputMapper(
                mapper,
                options,
                enabled
            );
        }
    }
}
