namespace Tetrominoes.Audio
{
    public interface IAudioService
    {
        IMusicService Music { get; }
        ISoundService Sound { get; }
    }
}
