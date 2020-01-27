using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Tetrominoes.Audio
{
    public class SoundService : ISoundService
    {
        readonly GameComponent _host;
        public SoundService(GameComponent host)
        {
            _host = host ?? throw new ArgumentNullException(nameof(host));
            Queue = new Queue<SoundEffect>();

            _soundEffects = new Dictionary<Sound, SoundEffect>();
            foreach (var sound in Enum.GetValues(typeof(Sound)).Cast<Sound>())
            {
                _soundEffects[sound] = host.Game.Content.Load<SoundEffect>(
                    Path.Combine("Sounds", sound.ToString())
                );
            }
        }

        public Queue<SoundEffect> Queue { get; }

        readonly Dictionary<Sound, SoundEffect> _soundEffects;
        public void Play(Sound sound)
        {
            _host.Enabled = true;
            Queue.Enqueue(_soundEffects[sound]);
        }
    }
}
