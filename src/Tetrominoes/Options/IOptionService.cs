#nullable disable

namespace Tetrominoes.Options
{
    public interface IOptionService
    {
        GameOptions Options { get; }

        void Reload();
        void Save();
    }
}
