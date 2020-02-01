#nullable disable

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using Tetrominoes.Audio;
using Tetrominoes.Graphics;
using Tetrominoes.Input;

namespace Tetrominoes.Options
{
    public class OptionEditorComponent : DrawableGameComponent, IOptionEditorService
    {
        public OptionEditorComponent(Game game)
            : base(game)
        {
            Enabled = Visible = false;
        }

        public static OptionEditorComponent AddTo(Game game)
        {
            var component = new OptionEditorComponent(game);
            game.Components.Add(component);
            game.Services.AddService<IOptionEditorService>(component);
            return component;
        }

        TimeSpan _delay;
        public void Hide()
        {
            Enabled = Visible = false;
        }

        public void Show()
        {
            _delay = TimeSpan.FromMilliseconds(300);
            _model = new OptionModel(_options.Options);
            _selectedOptionIndex = 0;
            Enabled = Visible = true;
        }

        IInputService _input;
        OptionModel _model;
        int _selectedOptionIndex;
        IMenuService _menu;
        IAudioService _audio;
        IOptionService _options;
        GraphicsDeviceManager _manager;
        public override void Initialize()
        {
            _input = Game.Services.GetService<IInputService>();
            _menu = Game.Services.GetService<IMenuService>();
            _audio = Game.Services.GetService<IAudioService>();
            _options = Game.Services.GetService<IOptionService>();
            _manager = Game.Services.GetService<GraphicsDeviceManager>();

            base.Initialize();
        }

        struct Resolution
        {
            public uint Width { get; }
            public uint Height { get; }
            public Resolution(uint width, uint height)
            {
                Width = width;
                Height = height;
            }
            public override string ToString() =>
                $"{Width}x{Height}";
        }

        class OptionModel
        {
            readonly GameOptions _root;
            readonly OptionGameplayModel _gameplay;
            readonly OptionGraphicsModel _graphics;
            readonly OptionAudioModel _audio;
            readonly OptionInputKeyboardModel _keyboard;
            readonly OptionInputGamePadModel _gamePad;

            public IReadOnlyList<IOption> Options { get; }

            public OptionModel(GameOptions options)
            {
                _root = options ?? throw new ArgumentNullException(nameof(options));
                _gameplay = new OptionGameplayModel(options.Gameplay);
                _graphics = new OptionGraphicsModel(options.Graphics);
                _audio = new OptionAudioModel(options.Audio);
                _keyboard = new OptionInputKeyboardModel(options.Input.Keyboard);
                _gamePad = new OptionInputGamePadModel(options.Input.GamePad);

                Options = new IOption[]
                {
                    new OptionHeader("Back"),
                    new OptionHeader("Save"),
                    _gameplay.Header,
                    _gameplay.ShadowPiece,
                    _gameplay.SwapPiece,
                    _gameplay.SlideSpeed,
                    _graphics.Header,
                    _graphics.Resolution,
                    _graphics.Fullscreen,
                    _audio.Header,
                    _audio.MusicVolume,
                    _audio.SoundVolume,
                    new OptionHeader("Input"),
                    _keyboard.Header,
                    _keyboard.Enabled,
                    _keyboard.Up,
                    _keyboard.Down,
                    _keyboard.Left,
                    _keyboard.Right,
                    _keyboard.RotateLeft,
                    _keyboard.RotateRight,
                    _keyboard.Drop,
                    _keyboard.Swap,
                    _keyboard.Pause,
                    _keyboard.Back,
                    _gamePad.Header,
                    _gamePad.Enabled,
                    _gamePad.Up,
                    _gamePad.Down,
                    _gamePad.Left,
                    _gamePad.Right,
                    _gamePad.RotateLeft,
                    _gamePad.RotateRight,
                    _gamePad.Drop,
                    _gamePad.Swap,
                    _gamePad.Pause,
                    _gamePad.Back,
                };
            }

            public void Commit()
            {
                _gameplay.Commit();
                _graphics.Commit();
                _audio.Commit();
                _keyboard.Commit();
                _gamePad.Commit();
            }

            class OptionGameplayModel
            {
                readonly GameGameplayOptions _options;
                public OptionHeader Header { get; }
                public OptionToggle ShadowPiece { get; }
                public OptionToggle SwapPiece { get; }
                public OptionItemList<SlideSpeed> SlideSpeed { get; }

                public OptionGameplayModel(GameGameplayOptions options)
                {
                    _options = options ?? throw new ArgumentNullException(nameof(options));
                    Header = new OptionHeader("Gameplay");
                    ShadowPiece = new OptionToggle(" Shadow Piece") { SelectedValue = options.ShadowPiece };
                    SwapPiece = new OptionToggle(" Swap Piece") { SelectedValue = options.SwapPiece };
                    SlideSpeed = new OptionItemList<SlideSpeed>(
                        " Slide Speed",
                        options.SlideSpeed,
                        new[] {
                            Tetrominoes.Options.SlideSpeed.Off,
                            Tetrominoes.Options.SlideSpeed.Slow,
                            Tetrominoes.Options.SlideSpeed.Fast,
                            Tetrominoes.Options.SlideSpeed.Instant
                        }
                    );
                }

                public void Commit()
                {
                    _options.ShadowPiece = ShadowPiece.SelectedValue;
                    _options.SwapPiece = SwapPiece.SelectedValue;
                    _options.SlideSpeed = SlideSpeed.SelectedValue;
                }
            }

            class OptionGraphicsModel
            {
                readonly GameGraphicsOptions _options;
                public OptionHeader Header { get; }
                public OptionItemList<Resolution> Resolution { get; }
                public OptionToggle Fullscreen { get; }

                public OptionGraphicsModel(GameGraphicsOptions options)
                {
                    _options = options ?? throw new ArgumentNullException(nameof(options));
                    Header = new OptionHeader("Graphics");
                    Resolution = new OptionItemList<Resolution>(
                        " Resolution",
                        new Resolution(options.Width, options.Height),
                        new[] {

                            // 16:9
                            new Resolution(1280, 720),
                            new Resolution(1366, 768),
                            new Resolution(1600, 900),
                            new Resolution(1920, 1080),
                            new Resolution(2560, 1440),

                            // 16:10
                            new Resolution(1280, 800),
                            new Resolution(1440, 900),
                            new Resolution(1680, 1050),

                            // 4:3
                            new Resolution(800, 600),
                            new Resolution(1024, 768),
                            new Resolution(1280, 1024),
                        }
                    );
                    Fullscreen = new OptionToggle(" Fullscreen")
                    {
                        SelectedValue = options.Fullscreen
                    };
                }

                public void Commit()
                {
                    _options.Fullscreen = Fullscreen.SelectedValue;
                    _options.Width = Resolution.SelectedValue.Width;
                    _options.Height = Resolution.SelectedValue.Height;
                }
            }

            class OptionAudioModel
            {
                readonly GameAudioOptions _options;
                public OptionHeader Header { get; }
                public OptionPercentage MusicVolume { get; }
                public OptionPercentage SoundVolume { get; }

                public OptionAudioModel(GameAudioOptions options)
                {
                    _options = options ?? throw new ArgumentNullException(nameof(options));
                    Header = new OptionHeader("Audio");
                    MusicVolume = new OptionPercentage(" Music Volume", options.MusicVolume);
                    SoundVolume = new OptionPercentage(" Sound Volume", options.SoundVolume);
                }

                public void Commit()
                {
                    _options.MusicVolume = MusicVolume.SelectedValue;
                    _options.SoundVolume = SoundVolume.SelectedValue;
                }
            }

            class OptionInputKeyboardModel : IInput<OptionEnum<Keys>>
            {
                readonly GameInputKeyboardOptions _options;
                public OptionHeader Header { get; }
                public OptionToggle Enabled { get; }
                public OptionEnum<Keys> Up { get; }
                public OptionEnum<Keys> Down { get; }
                public OptionEnum<Keys> Left { get; }
                public OptionEnum<Keys> Right { get; }
                public OptionEnum<Keys> RotateLeft { get; }
                public OptionEnum<Keys> RotateRight { get; }
                public OptionEnum<Keys> Drop { get; }
                public OptionEnum<Keys> Swap { get; }
                public OptionEnum<Keys> Pause { get; }
                public OptionEnum<Keys> Back { get; }

                public OptionInputKeyboardModel(GameInputKeyboardOptions options)
                {
                    _options = options ?? throw new ArgumentNullException(nameof(options));
                    Header = new OptionHeader(" Keyboard");
                    Enabled = new OptionToggle("  Enabled")
                    {
                        SelectedValue = options.Enabled
                    };
                    Up = new OptionEnum<Keys>("  Up", options.Up);
                    Down = new OptionEnum<Keys>("  Down", options.Down);
                    Left = new OptionEnum<Keys>("  Left", options.Left);
                    Right = new OptionEnum<Keys>("  Right", options.Right);
                    RotateLeft = new OptionEnum<Keys>("  Rotate Left", options.RotateLeft);
                    RotateRight = new OptionEnum<Keys>("  Rotate Right", options.RotateRight);
                    Drop = new OptionEnum<Keys>("  Drop", options.Drop);
                    Swap = new OptionEnum<Keys>("  Swap", options.Swap);
                    Pause = new OptionEnum<Keys>("  Pause", options.Pause);
                    Back = new OptionEnum<Keys>("  Back", options.Back);
                }

                public void Commit()
                {
                    _options.Enabled = Enabled.SelectedValue;
                    _options.Up = Up.SelectedValue;
                    _options.Down = Down.SelectedValue;
                    _options.Left = Left.SelectedValue;
                    _options.Right = Right.SelectedValue;
                    _options.RotateLeft = RotateLeft.SelectedValue;
                    _options.RotateRight = RotateRight.SelectedValue;
                    _options.Drop = Drop.SelectedValue;
                    _options.Swap = Swap.SelectedValue;
                    _options.Pause = Pause.SelectedValue;
                    _options.Back = Back.SelectedValue;
                }
            }

            class OptionInputGamePadModel : IInput<OptionEnum<GamePadButtonTypes>>
            {
                readonly GameInputGamePadOptions _options;
                public OptionHeader Header { get; }
                public OptionToggle Enabled { get; }
                public OptionEnum<GamePadButtonTypes> Up { get; }
                public OptionEnum<GamePadButtonTypes> Down { get; }
                public OptionEnum<GamePadButtonTypes> Left { get; }
                public OptionEnum<GamePadButtonTypes> Right { get; }
                public OptionEnum<GamePadButtonTypes> RotateLeft { get; }
                public OptionEnum<GamePadButtonTypes> RotateRight { get; }
                public OptionEnum<GamePadButtonTypes> Drop { get; }
                public OptionEnum<GamePadButtonTypes> Swap { get; }
                public OptionEnum<GamePadButtonTypes> Pause { get; }
                public OptionEnum<GamePadButtonTypes> Back { get; }

                public OptionInputGamePadModel(GameInputGamePadOptions options)
                {
                    _options = options ?? throw new ArgumentNullException(nameof(options));
                    Header = new OptionHeader(" Gamepad");
                    Enabled = new OptionToggle("  Enabled")
                    {
                        SelectedValue = options.Enabled
                    };
                    Up = new OptionEnum<GamePadButtonTypes>("  Up", options.Up, ConvertToString);
                    Down = new OptionEnum<GamePadButtonTypes>("  Down", options.Down, ConvertToString);
                    Left = new OptionEnum<GamePadButtonTypes>("  Left", options.Left, ConvertToString);
                    Right = new OptionEnum<GamePadButtonTypes>("  Right", options.Right, ConvertToString);
                    RotateLeft = new OptionEnum<GamePadButtonTypes>("  Rotate Left", options.RotateLeft, ConvertToString);
                    RotateRight = new OptionEnum<GamePadButtonTypes>("  Rotate Right", options.RotateRight, ConvertToString);
                    Drop = new OptionEnum<GamePadButtonTypes>("  Drop", options.Drop, ConvertToString);
                    Swap = new OptionEnum<GamePadButtonTypes>("  Swap", options.Swap, ConvertToString);
                    Pause = new OptionEnum<GamePadButtonTypes>("  Pause", options.Pause, ConvertToString);
                    Back = new OptionEnum<GamePadButtonTypes>("  Back", options.Back, ConvertToString);
                }

                static string ConvertToString(GamePadButtonTypes type) =>
                    _map[type];
                static readonly Dictionary<GamePadButtonTypes, string> _map = 
                    new Dictionary<GamePadButtonTypes, string>
                    {
                        [GamePadButtonTypes.ButtonsA] = "×",
                        [GamePadButtonTypes.ButtonsB] = "Ö",
                        [GamePadButtonTypes.ButtonsBack] = "Ï",
                        [GamePadButtonTypes.ButtonsBigButton] = "æ",
                        [GamePadButtonTypes.ButtonsLeftShoulder] = "Í",
                        [GamePadButtonTypes.ButtonsLeftStick] = "Ú",
                        [GamePadButtonTypes.ButtonsRightShoulder] = "Î",
                        [GamePadButtonTypes.ButtonsRightStick] = "Û",
                        [GamePadButtonTypes.ButtonsStart] = "Ð",
                        [GamePadButtonTypes.ButtonsX] = "Ô",
                        [GamePadButtonTypes.ButtonsY] = "Õ",
                        [GamePadButtonTypes.DPadDown] = "Á",
                        [GamePadButtonTypes.DPadLeft] = "Â",
                        [GamePadButtonTypes.DPadRight] = "Ã",
                        [GamePadButtonTypes.DPadUp] = "À",
                        [GamePadButtonTypes.TriggersLeft] = "Ë",
                        [GamePadButtonTypes.TriggersRight] = "Ì",
                    };

                public void Commit()
                {
                    _options.Enabled = Enabled.SelectedValue;
                    _options.Up = Up.SelectedValue;
                    _options.Down = Down.SelectedValue;
                    _options.Left = Left.SelectedValue;
                    _options.Right = Right.SelectedValue;
                    _options.RotateLeft = RotateLeft.SelectedValue;
                    _options.RotateRight = RotateRight.SelectedValue;
                    _options.Drop = Drop.SelectedValue;
                    _options.Swap = Swap.SelectedValue;
                    _options.Pause = Pause.SelectedValue;
                    _options.Back = Back.SelectedValue;
                }
            }
        }

        SpriteBatch _spriteBatch;
        UIFonts _uiFonts;
        GamePadFonts _gamePadFonts;
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _uiFonts = new UIFonts(FontSize.Large, Game.Content);
            _gamePadFonts = new GamePadFonts(FontSize.Large, Game.Content);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            // Delay to prevent input from prior menu.
            _delay -= gameTime.ElapsedGameTime;
            if (_delay > TimeSpan.Zero)
            {
                return;
            }

            if (_inputMonitor != default)
            {
                if (_inputMonitor.TryUpdate())
                {
                    _inputMonitor = default;
                }
                return;
            }

            var state = _input.State;
            HandleOptionSelection(state);
            HandleOptionValue(state);

            base.Update(gameTime);
        }

        IInputMonitor _inputMonitor;
        interface IInputMonitor
        {
            bool TryUpdate();
        }

        class GamePadInputMonitor : IInputMonitor
        {
            readonly PlayerIndex _index;
            readonly OptionEnum<GamePadButtonTypes> _option;
            public GamePadInputMonitor(PlayerIndex index, OptionEnum<GamePadButtonTypes> option)
            {
                _index = index;
                _option = option ?? throw new ArgumentNullException(nameof(option));
            }

            GamePadState _last;
            GamePadState _current;
            static Buttons[] _available = new[]
            {
                Buttons.DPadDown,
                Buttons.DPadLeft,
                Buttons.DPadRight,
                Buttons.DPadUp,
                Buttons.Start,
                Buttons.Back,
                Buttons.LeftStick,
                Buttons.RightStick,
                Buttons.LeftShoulder,
                Buttons.RightShoulder,
                Buttons.BigButton,
                Buttons.A,
                Buttons.B,
                Buttons.X,
                Buttons.Y,
                Buttons.LeftTrigger,
                Buttons.RightTrigger
            };
            static GamePadButtonTypes[] _types = new[]
            {
                GamePadButtonTypes.DPadDown,
                GamePadButtonTypes.DPadLeft,
                GamePadButtonTypes.DPadRight,
                GamePadButtonTypes.DPadUp,
                GamePadButtonTypes.ButtonsStart,
                GamePadButtonTypes.ButtonsBack,
                GamePadButtonTypes.ButtonsLeftStick,
                GamePadButtonTypes.ButtonsRightStick,
                GamePadButtonTypes.ButtonsLeftShoulder,
                GamePadButtonTypes.ButtonsRightShoulder,
                GamePadButtonTypes.ButtonsBigButton,
                GamePadButtonTypes.ButtonsA,
                GamePadButtonTypes.ButtonsB,
                GamePadButtonTypes.ButtonsX,
                GamePadButtonTypes.ButtonsY,
                GamePadButtonTypes.TriggersLeft,
                GamePadButtonTypes.TriggersRight
            };
            bool IsPressed(Buttons buttons) =>
                _last.IsButtonDown(buttons) && _current.IsButtonUp(buttons);
            public bool TryUpdate()
            {
                _current = GamePad.GetState(_index);

                for (var i = 0; i < _available.Length; i++)
                {
                    if (IsPressed(_available[i]))
                    {
                        _option.SelectedValue = _types[i];
                        return true;
                    }
                }
                
                _last = _current;
                return false;
            }
        }

        class KeyboardInputMonitor : IInputMonitor
        {
            Keys[] _last = Array.Empty<Keys>();
            Keys[] _current = Array.Empty<Keys>();
            readonly OptionEnum<Keys> _option;
            public KeyboardInputMonitor(OptionEnum<Keys> option) =>
                _option = option ?? throw new ArgumentNullException(nameof(option));

            public bool TryUpdate()
            {
                var state = Keyboard.GetState();
                _current = state.GetPressedKeys();

                if (_current.Length == 0 && _last.Length > 0)
                {
                    _option.SelectedValue = _last[0];
                    return true;
                }

                _last = _current;
                return false;
            }
        }

        void HandleOptionValue(InputState state)
        {
            if (state.Back.State == InputButtonState.Pressed)
            {
                Hide();
                _menu.Show();
                return;
            }

            var offset = 0;
            if (state.Left.State == InputButtonState.Pressed ||
                state.RotateLeft.State == InputButtonState.Pressed)
            {
                offset--;
            }
            if (state.Right.State == InputButtonState.Pressed ||
                state.Drop.State == InputButtonState.Pressed ||
                state.Pause.State == InputButtonState.Pressed ||
                state.RotateRight.State == InputButtonState.Pressed ||
                state.Swap.State == InputButtonState.Pressed)
            {
                offset++;
            }

            if (offset == 0) return;

            var shouldPlay = false;
            var option = _model.Options[_selectedOptionIndex];
            switch (option)
            {
                case OptionToggle toggle:
                    shouldPlay = true;
                    toggle.SelectedValue = !toggle.SelectedValue;
                    break;

                case OptionHeader header:
                    if (header.Name == "Back")
                    {
                        shouldPlay = true;
                        Hide();
                        _menu.Show();
                    }
                    if (header.Name == "Save")
                    {
                        shouldPlay = true;
                        _model.Commit();
                        _options.Save();

                        HandleGraphicsChange();

                        Hide();
                        _menu.Show();
                    }
                    break;

                case OptionPercentage percentage:
                    shouldPlay = true;
                    percentage.SelectedValue += offset;
                    break;

                case OptionItemList<string> stringList:
                    shouldPlay = true;
                    stringList.Next(offset);
                    break;

                case OptionItemList<Resolution> resolutionList:
                    shouldPlay = true;
                    resolutionList.Next(offset);
                    break;

                case OptionItemList<SlideSpeed> slideSpeedList:
                    shouldPlay = true;
                    slideSpeedList.Next(offset);
                    break;

                case OptionEnum<Keys> keysEnum:
                    shouldPlay = true;
                    _inputMonitor = new KeyboardInputMonitor(keysEnum);
                    break;

                case OptionEnum<GamePadButtonTypes> buttonEnum:
                    shouldPlay = true;
                    _inputMonitor = new GamePadInputMonitor(
                        PlayerIndex.One,
                        buttonEnum
                    );
                    break;
            }
            if (shouldPlay)
            {
                _audio.Sound.Play(
                    offset < 0
                        ? Sound.RotateLeft
                        : Sound.RotateRight
                );
            }
        }

        void HandleGraphicsChange()
        {
            var graphics = _options.Options.Graphics;

            if (_manager.PreferredBackBufferWidth == graphics.Width &&
                _manager.PreferredBackBufferHeight == graphics.Height &&
                _manager.IsFullScreen == graphics.Fullscreen)
            {
                return;
            }

            _manager.PreferredBackBufferWidth = (int)graphics.Width;
            _manager.PreferredBackBufferHeight = (int)graphics.Height;
            _manager.IsFullScreen = graphics.Fullscreen;
            _manager.ApplyChanges();
        }

        void HandleOptionSelection(InputState state)
        {
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
                do
                {
                    _selectedOptionIndex += offset;
                    if (_selectedOptionIndex < 0)
                    {
                        _selectedOptionIndex = _model.Options.Count - 1;
                    }
                    else if (_selectedOptionIndex >= _model.Options.Count)
                    {
                        _selectedOptionIndex = 0;
                    }
                } while (ShouldSkipOption(_model.Options[_selectedOptionIndex]));
                _audio.Sound.Play(Sound.Move);
            }
        }

        static bool ShouldSkipOption(IOption option) =>
            option is OptionHeader header &&
                header.Name != "Back" &&
                header.Name != "Save";

        public override void Draw(GameTime gameTime)
        {
            var maxWidth = 0.0f;
            for (var i = 0; i < _model.Options.Count; i++)
            {
                var font = i == _selectedOptionIndex
                    ? _uiFonts.BoldWeight
                    : _uiFonts.NormalWeight;

                var option = _model.Options[i];

                maxWidth = Math.Max(
                    font.MeasureString(option.Name).X,
                    maxWidth
                );
            }

            var pp = GraphicsDevice.PresentationParameters;
            var tx = Matrix.CreateTranslation(
                ((pp.BackBufferWidth - maxWidth) / 2) - 384,
                384 + (_selectedOptionIndex * -64),
                0
            );
            
            _spriteBatch.Begin(transformMatrix: tx, samplerState: SamplerState.PointClamp);
            var pos = new Vector2();
            for (var i = 0; i < _model.Options.Count; i++)
            {
                var selected = i == _selectedOptionIndex;
                var font = selected
                    ? _uiFonts.BoldWeight
                    : _uiFonts.NormalWeight;

                var option = _model.Options[i];
                var measurement = font.MeasureString(option.Name);

                pos.X = 0;
                _spriteBatch.DrawString(
                    font,
                    option.Name,
                    pos,
                    Color.Black
                );

                pos.X = maxWidth + 64;
                var valueFont = option is OptionEnum<GamePadButtonTypes>
                    ? _gamePadFonts.NormalWeight
                    : font;
                var valueText = option.ToString();
                _spriteBatch.DrawString(
                    valueFont,
                    valueText,
                    pos,
                    Color.Black
                );

                if (selected && _inputMonitor != null)
                {
                    pos.X += valueFont.MeasureString(valueText).X + 32;

                    var size = font.MeasureString("¤") / 2;

                    _spriteBatch.DrawString(
                        font,
                        "¤",
                        pos + size,
                        Color.Black,
                        MathHelper.TwoPi * (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds),
                        size,
                        1.0f,
                        SpriteEffects.None,
                        1.0f
                    );
                }

                pos.Y += measurement.Y;
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
