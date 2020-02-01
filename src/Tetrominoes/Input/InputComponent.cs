#nullable disable

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Tetrominoes.Options;

namespace Tetrominoes.Input
{
    public class InputComponent : GameComponent, IInputService
    {
        public InputState State { get; private set; }

        public InputComponent(Game game): base(game)
        {
        }

        public static InputComponent AddTo(Game game)
        {
            var component = new InputComponent(game);
            game.Services.AddService<IInputService>(component);
            game.Components.Add(component);
            return component;
        }

        IOptionService _options;
        //IAlertService _alertService;
        readonly List<IInputMapper> _enabled = new List<IInputMapper>();
        readonly List<IInputMapper> _disabled = new List<IInputMapper>();
        public override void Initialize()
        {
            //_alertService = Game.Services.GetService<IAlertService>();
            _options = Game.Services.GetService<IOptionService>();
            _enabled.Add(
                new KeyboardInputMapper(_options)
                    .HandleEnabledChange(_options, KeyboardInputMapper.IsEnabled)
            );
            _disabled.Add(
                new GamePadInputMapper(_options)
                    .HandleEnabledChange(_options, GamePadInputMapper.IsEnabled)
            );

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            State = new InputState();

            var index = _enabled.Count;
            while (--index >= 0)
            {
                var mapper = _enabled[index];
                var state = mapper.Update(gameTime);
                if (state == InputConnection.Disconnected)
                {
                    _enabled.RemoveAt(index);
                    _disabled.Add(mapper);

                    //_alertService.Display($"{mapper} DISCONNECTED", default);
                }
                else
                {
                    State += mapper.Current;
                }
            }

            index = _disabled.Count;
            while (--index >= 0)
            {
                var mapper = _disabled[index];
                var state = mapper.GetConnectionState();
                if (state == InputConnection.Connected)
                {
                    _disabled.RemoveAt(index);
                    _enabled.Add(mapper);

                    //_alertService.Display($"{mapper} CONNECTED", default);
                }
            }

            base.Update(gameTime);
        }
    }
}
