using System;
using Tetrominoes.Options;

namespace Tetrominoes.Input
{
    public static class InputMapperExtensions
    {
        public static IInputMapper HandleEnabledChange(this IInputMapper mapper, IOptionService options, Func<IOptionService, bool> enabled)
        {
            if (mapper == default) throw new ArgumentNullException(nameof(mapper));
            if (options == default) throw new ArgumentNullException(nameof(options));
            if (enabled == default) throw new ArgumentNullException(nameof(enabled));

            return new EnabledWrapperInputMapper(
                mapper,
                options,
                enabled
            );
        }
    }
}
