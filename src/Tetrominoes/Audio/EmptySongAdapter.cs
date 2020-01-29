namespace Tetrominoes.Audio
{
    public class EmptySongAdapter : ISongAdapter
    {
        public static ISongAdapter Instance { get; } = new EmptySongAdapter();

        public void SetPitch(float value) { }
    }
}
