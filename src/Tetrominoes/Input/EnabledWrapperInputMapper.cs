using Microsoft.Xna.Framework;
using System;
using Tetrominoes.Options;

namespace Tetrominoes.Input
{
    public class EnabledWrapperInputMapper : IInputMapper
    {
        readonly IInputMapper _inner;
        readonly IOptionService _options;
        readonly Func<IOptionService, bool> _enabled;
        public EnabledWrapperInputMapper(IInputMapper inner, IOptionService options, Func<IOptionService, bool> enabled)
        {
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _enabled = enabled ?? throw new ArgumentNullException(nameof(enabled));
        }

        public InputState Current => _inner.Current;

        public InputConnection GetConnectionState() =>
            _enabled(_options)
                ? _inner.GetConnectionState()
                : InputConnection.Disconnected;

        public InputConnection Update(GameTime gameTime) =>
            _enabled(_options)
                ? _inner.Update(gameTime)
                : InputConnection.Disconnected;

        public override string ToString() =>
            _inner.ToString() ?? "";
    }
}
