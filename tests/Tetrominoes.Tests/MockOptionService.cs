using System;
using Tetrominoes.Options;

namespace Tetrominoes.Tests
{
    class MockOptionService : IOptionService
    {
        public GameOptions Options { get; } = new GameOptions();

        public event Action<GameOptions>? Updated;

        public void Reload()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
