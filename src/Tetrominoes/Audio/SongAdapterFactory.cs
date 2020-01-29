using Microsoft.Xna.Framework.Media;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Tetrominoes.Audio
{
    public static class SongAdapterFactory
    {
        static Func<Song, int>? GetGetSourceIdFunc()
        {
            // Runtime checks to make sure internal APIs have not changed.
            // Note that this is definitely not compatible with all MonoGame implementations.
            var streamField = typeof(Song).GetField("stream", BindingFlags.NonPublic | BindingFlags.Instance);
            if (streamField == default) return default;

            var oggStreamType = typeof(Song).Assembly.GetType("Microsoft.Xna.Framework.Audio.OggStream");
            if (oggStreamType == default) return default;

            var alSourceIdField = oggStreamType.GetField("alSourceId", BindingFlags.NonPublic | BindingFlags.Instance);
            if (alSourceIdField == default) return default;

            if (alSourceIdField.FieldType != typeof(int)) return default;

            if (streamField != null)
            {
                // song => song.stream.alSourceId
                var song = Expression.Parameter(typeof(Song), "song");
                var getStream = Expression.Field(song, streamField);
                var getAlSourceId = Expression.Field(getStream, alSourceIdField);
                var lambda = Expression.Lambda<Func<Song, int>>(getAlSourceId, song);

                return lambda.Compile();
            }

            return default;
        }

        static Action<int, int, float>? GetSetSourceFunc()
        {
            var oggStreamType = typeof(Song).Assembly.GetType("Microsoft.Xna.Framework.Audio.OggStream");
            if (oggStreamType == default) return default;

            var alSourcefType = typeof(Song).Assembly.GetType("MonoGame.OpenAL.ALSourcef");
            if (alSourcefType == default) return default;

            var alType = typeof(Song).Assembly.GetType("MonoGame.OpenAL.AL");
            if (alType == default) return default;

            var sourceParameterTypes = new[]
            {
                typeof(int),
                alSourcefType,
                typeof(float)
            };
            var alSourceMethod = alType.GetMethod("Source", BindingFlags.NonPublic | BindingFlags.Static, default, sourceParameterTypes, default);
            if (alSourceMethod == default) return default;

            // (sourceId, func, dist) => AL.Source(sourceId, (ALSourcef)func, dist)

            var sourceId = Expression.Parameter(typeof(int), "sourceId");
            var func = Expression.Parameter(typeof(int), "func");
            var dist = Expression.Parameter(typeof(float), "dist");

            var body = Expression.Call(
                alSourceMethod, 
                sourceId, 
                Expression.Convert(func, alSourcefType), 
                dist
            );
            var lambda = Expression.Lambda<Action<int, int, float>>(
                body, 
                sourceId, 
                func, 
                dist
            );
            return lambda.Compile();
        }

        static readonly Func<Song, ISongAdapter> _factory;
        public static ISongAdapter Create(Song song) =>
            _factory(song);

        static SongAdapterFactory()
        {
            _factory = _ => EmptySongAdapter.Instance;

            try
            {
                var getSourceId = GetGetSourceIdFunc();
                if (getSourceId == default) return;

                var setSource = GetSetSourceFunc();
                if (setSource == default) return;

                _factory = song => new OggSongAdapter(song, getSourceId, setSource);
            }
            catch
            {
                // Ignore the exception to prevent the app from not starting,
                // perhaps at a later time log it if for some reason the above fails
            }
        }
    }
}
