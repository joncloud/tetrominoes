#nullable disable

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Tetrominoes.Audio;
using Tetrominoes.Input;
using Tetrominoes.Options;

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
        IOptionEditorService _optionEditor;
        IBackgroundService _background;
        public override void Initialize()
        {
            _input = Game.Services.GetService<IInputService>();
            _match = Game.Services.GetService<IMatchService>();
            _audio = Game.Services.GetService<IAudioService>();
            _optionEditor = Game.Services.GetService<IOptionEditorService>();
            _background = Game.Services.GetService<IBackgroundService>();

            _options = new[]
            {
                new MenuOption(NewGame, "New Game", new Vector2()),
                new MenuOption(ShowOptions, "Options", new Vector2(0, 64)),
                new MenuOption(Game.Exit, "Quit", new Vector2(0, 128))
            };

            base.Initialize();
        }

        void NewGame()
        {
            _match.NewMatch();
            Hide();
        }

        void ShowOptions()
        {
            _optionEditor.Show();
            Hide();
        }

        SpriteBatch _spriteBatch;
        SpriteFont _normalWeight;
        SpriteFont _boldWeight;
        Random _random;
        Texture2D _tileTexture;
        const int TileWidth = 8;
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _normalWeight = Game.Content.Load<SpriteFont>("Fonts/UI");
            _boldWeight = Game.Content.Load<SpriteFont>("Fonts/UI-Bold");
            _tileTexture = Game.Content.Load<Texture2D>("Textures/Tiles");

#if DEBUG
            _random = new Random(0);
#else
            _random = new Random();
#endif
            RandomizeGrid();

            base.LoadContent();
        }

        static readonly Color[] _tetrominoColors = new[]
        {
            Color.Transparent,
            Color.Cyan,
            Color.Yellow,
            Color.Magenta,
            Color.Blue,
            Color.Orange,
            Color.Lime,
            Color.Red
        };
        TimeSpan _tillNextRandom;
        void RandomizeGrid()
        {
            _background.BackgroundEffect.Effect = Game.Content.Load<Effect>(
                _random.NextElement(BackgroundEffect.EffectNames)
            );
            _tillNextRandom = TimeSpan.FromSeconds(15);

            var tx = Matrix.CreateTranslation(16, 0, 0);

            GraphicsDevice.SetRenderTarget(_background.Board);
            GraphicsDevice.Clear(Color.White);
            _spriteBatch.Begin(transformMatrix: tx, samplerState: SamplerState.PointClamp);

            for (var i = 0; i < MatchGrid.Size; i++)
            {
                if (_random.NextChance(25)) continue;
                var type = _random.NextEnum<TetrominoPiece>();
                if (type == TetrominoPiece.Empty) continue;

                var color = _tetrominoColors[(int)type];

                var point = MatchGrid.GetPosition(i).ToVector2() * TileWidth;
                _spriteBatch.Draw(
                    _tileTexture,
                    point,
                    new Rectangle(0, 0, TileWidth, TileWidth),
                    color
                );
            }

            _spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
        }

        public override void Update(GameTime gameTime)
        {
            _tillNextRandom -= gameTime.ElapsedGameTime;
            if (_tillNextRandom <= TimeSpan.Zero)
            {
                RandomizeGrid();
            }

            var state = _input.State;

            var offset = 0;
            if (state.Up.State == InputButtonState.Pressed)
            {
                offset--;
            }
            if (state.Down.State == InputButtonState.Pressed)
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
                state.RotateLeft.State,
                InputMath.GreaterOf(
                    state.RotateRight.State,
                    InputMath.GreaterOf(
                        state.Swap.State,
                        InputMath.GreaterOf(
                            state.Drop.State,
                            InputMath.GreaterOf(
                                state.Drop.State,
                                state.Pause.State
                            )
                        )
                    )
                )
            );
            if (chosenState == InputButtonState.Pressed)
            {
                _audio.Sound.Play(Sound.Drop);
                _options[_selectedOptionIndex].Choose();
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            RenderMenu();
            RenderAppVersion();

            base.Draw(gameTime);
        }

        void RenderMenu()
        {
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
        }

        void RenderAppVersion()
        {
            var pp = GraphicsDevice.PresentationParameters;
            var tx = Matrix.CreateScale(0.5f, 0.5f, 1) * Matrix.CreateTranslation(
                16,
                (pp.BackBufferHeight - 48),
                0
            );
            _spriteBatch.Begin(transformMatrix: tx, samplerState: SamplerState.PointClamp);
            _spriteBatch.DrawString(
                _normalWeight,
                AppVersion.Display,
                Vector2.Zero,
                Color.Black
            );
            _spriteBatch.End();
        }
    }
}
