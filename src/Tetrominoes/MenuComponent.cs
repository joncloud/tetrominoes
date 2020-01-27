using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Tetrominoes.Audio;
using Tetrominoes.Input;

namespace Tetrominoes
{
    public class MenuComponent : DrawableGameComponent, IMenuService
    {
        public MenuComponent(Game game) : base(game)
        {
        }

        public static MenuComponent AddTo(Game game)
        {
            var component = new MenuComponent(game);
            game.Components.Add(component);
            game.Services.AddService<IMenuService>(component);
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
        MenuOption[] _options;
        int _selectedOptionIndex;
        IMatchService _match;
        IAudioService _audio;
        public override void Initialize()
        {
            _input = Game.Services.GetService<IInputService>();
            _match = Game.Services.GetService<IMatchService>();
            _audio = Game.Services.GetService<IAudioService>();

            _options = new[]
            {
                new MenuOption(NewGame, "New Game", new Vector2()),
                new MenuOption(Game.Exit, "Quit", new Vector2(0, 64))
            };

            base.Initialize();
        }

        void NewGame()
        {
            _match.NewMatch();
            Hide();
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

            var chosenState = InputMath.GreaterOf(
                state.RotateLeft,
                InputMath.GreaterOf(
                    state.RotateRight,
                    InputMath.GreaterOf(
                        state.Swap,
                        InputMath.GreaterOf(
                            state.Drop,
                            InputMath.GreaterOf(
                                state.Drop,
                                state.Pause
                            )
                        )
                    )
                )
            );
            if (chosenState == InputButtonState.Pressed)
            {
                _options[_selectedOptionIndex].Choose();
            }

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
                    font.MeasureString(option.Text).X,
                    maxWidth
                );
            }

            var pp = GraphicsDevice.PresentationParameters;
            var tx = Matrix.CreateTranslation(
                (pp.BackBufferWidth - maxWidth) / 2,
                (pp.BackBufferHeight - 64) / 2,
                0
            );

            _spriteBatch.Begin(transformMatrix: tx, samplerState: SamplerState.PointClamp);
            for (var i = 0; i < _options.Length; i++)
            {
                var font = i == _selectedOptionIndex
                    ? _boldWeight
                    : _normalWeight;

                var option = _options[i];

                _spriteBatch.DrawString(
                    font,
                    option.Text,
                    option.Position,
                    Color.Black
                );
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
