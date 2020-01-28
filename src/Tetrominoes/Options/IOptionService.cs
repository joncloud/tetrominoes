using System;

namespace Tetrominoes.Options
{
    public interface IOptionService
    {
        event Action<GameOptions> Updated;

        GameOptions Options { get; }

        void Reload();
        void Save();
    }
}
