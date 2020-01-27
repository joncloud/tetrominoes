#nullable disable

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using Tetrominoes.Options;

namespace Tetrominoes.Audio
{
    public class AudioComponent : DrawableGameComponent, IAudioService
    {
        public AudioComponent(Game game) 
            : base(game)
        {
            Enabled = Visible = false;
        }

        public static AudioComponent AddTo(Game game)
        {
            var component = new AudioComponent(game);
            game.Components.Add(component);
            game.Services.AddService<IAudioService>(component);
            return component;
        }

        IOptionService _options;
        public override void Initialize()
        {
            //var messageService = Game.Services.GetService<IMessageService>();
            //messageService.Register<OptionsUpdated>(this);

            _options = Game.Services.GetService<IOptionService>();
            base.Initialize();
        }

        public IMusic Music { get; private set; }
        public ISound Sound { get; private set; }
        protected override void LoadContent()
        {
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = _options.Options.Audio.MusicVolumePercentage;

            Music = new MusicService(Game.Content);
            Sound = new SoundService(this);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (Sound.Queue.TryDequeue(out var effect))
            {
                effect.Play(
                    _options.Options.Audio.SoundVolumePercentage, 
                    0.0f, 
                    0.0f
                );
            }
            else
            {
                Enabled = false;
            }

            base.Update(gameTime);
        }

        //public void Handle(OptionsUpdated _)
        //{
        //    MediaPlayer.Volume = _options.Options.Audio.MusicVolumePercentage;
        //}
    }
}
