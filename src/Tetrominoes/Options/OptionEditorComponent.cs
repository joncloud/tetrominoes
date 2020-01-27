#nullable disable

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Tetrominoes.Audio;
using Tetrominoes.Input;

namespace Tetrominoes.Options
{
    public class OptionEditorComponent : DrawableGameComponent, IOptionEditorService
    {
        public OptionEditorComponent(Game game)
            : base(game)
        {
        }

        public static OptionEditorComponent AddTo(Game game)
        {
            var component = new OptionEditorComponent(game);
            game.Components.Add(component);
            game.Services.AddService<IOptionEditorService>(component);
            return component;
        }

        public void Hide()
        {
            Enabled = Visible = false;
        }

        public void Show()
        {
            Enabled = Visible = true;
        }

        IInputService _input;
        IOption[] _options;
        int _selectedOptionIndex;
        IMenuService _menu;
        IAudioService _audio;
        public override void Initialize()
        {
            _input = Game.Services.GetService<IInputService>();
            _menu = Game.Services.GetService<IMenuService>();
            _audio = Game.Services.GetService<IAudioService>();

            // TODO build based on options
            _options = new IOption[]
            {
                new OptionHeader("Back"),
                new OptionHeader("Graphics"),
                new OptionItemList<string>("Resolution", "1920x1080", new[] { "1920x1080" }),
                new OptionToggle("Resolution") { SelectedValue = true },
                new OptionHeader("Audio"),
                new OptionPercentage("Music Volume", 75),
                new OptionPercentage("Sound Volume", 100),
                new OptionHeader("Input"),
                new OptionHeader("Keyboard"),
                new OptionToggle("Enabled"),
                new OptionEnum<Keys>("Up", Keys.W),
                new OptionHeader("Gamepad"),
                new OptionEnum<GamePadButtonTypes>("Up", GamePadButtonTypes.DPadUp),
                new OptionToggle("Enabled")
            };

            base.Initialize();
        }


        SpriteBatch _spriteBatch;
        SpriteFont _normalWeight;
        SpriteFont _boldWeight;
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _normalWeight = Game.Content.Load<SpriteFont>("Fonts/UI");
            _boldWeight = Game.Content.Load<SpriteFont>("Fonts/UI-Bold");

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            var state = _input.State;

            var offset = 0;
            if (state.Up == InputButtonState.Pressed)
            {
                offset--;
            }
            if (state.Down == InputButtonState.Pressed)
            {
                offset++;
            }

            if (offset != 0)
            {
                _selectedOptionIndex += offset;
                if (_selectedOptionIndex < 0)
                {
                    _selectedOptionIndex = _options.Length - 1;
                }
                else if (_selectedOptionIndex >= _options.Length)
                {
                    _selectedOptionIndex = 0;
                }
                _audio.Sound.Play(Sound.Move);
            }

            // TODO allow changing options
            // TODO allow going back (and saving options)

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            var maxWidth = 0.0f;
            for (var i = 0; i < _options.Length; i++)
            {
                var font = i == _selectedOptionIndex
                    ? _boldWeight
                    : _normalWeight;

                var option = _options[i];

                maxWidth = Math.Max(
                    font.MeasureString(option.Name).X,
                    maxWidth
                );
            }

            var pp = GraphicsDevice.PresentationParameters;
            var tx = Matrix.CreateTranslation(
                (pp.BackBufferWidth - maxWidth) / 2,
                _selectedOptionIndex * -64,
                0
            );
            
            _spriteBatch.Begin(transformMatrix: tx, samplerState: SamplerState.PointClamp);
            var pos = new Vector2();
            for (var i = 0; i < _options.Length; i++)
            {
                var font = i == _selectedOptionIndex
                    ? _boldWeight
                    : _normalWeight;

                var option = _options[i];
                var measurement = font.MeasureString(option.Name);

                _spriteBatch.DrawString(
                    font,
                    option.Name,
                    pos,
                    Color.Black
                );

                pos.Y += measurement.Y;
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
