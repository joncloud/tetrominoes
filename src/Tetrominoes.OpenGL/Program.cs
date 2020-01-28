using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Tetrominoes.Audio;
using Tetrominoes.Input;
using Tetrominoes.Options;

namespace Tetrominoes.OpenGL
{
    class Program : Game
    {
        readonly GraphicsDeviceManager _manager;
        public Program()
        {
            _manager = new GraphicsDeviceManager(this);
            _manager.PreparingDeviceSettings += PreparingDeviceSettings;
            Content.RootDirectory = "Content";
        }

        static void Main(string[] args)
        {
            using var game = new Program();
            game.Run();
        }

        static void PreparingDeviceSettings(object? sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;
        }

        IOptionService? _options;
        protected override void Initialize()
        {
            Services.AddService(_manager);

            _options = OptionComponent.AddTo(this);
            var graphics = _options.Options.Graphics;
            if (graphics == null)
            {
                throw new InvalidOperationException("Ensure that [graphics] is set in config.toml.");
            }
            var pp = GraphicsDevice.PresentationParameters;
            pp.BackBufferWidth = (int)graphics.Width;
            pp.BackBufferHeight = (int)graphics.Height;
#if DEBUG
            pp.IsFullScreen = false;
#else
            pp.IsFullScreen = _options.Options.Graphics.Fullscreen;
#endif
            GraphicsDevice.Present();
            Window.Title = "Tetrominoes";

            InputComponent.AddTo(this);
            AudioComponent.AddTo(this);

            OptionEditorComponent.AddTo(this);
            MenuComponent.AddTo(this);
            MatchComponent.AddTo(this);
            base.Initialize();
        }

        KeyboardState _last;
        protected override void Update(GameTime gameTime)
        {
            var current = Keyboard.GetState();
            if (current.IsKeyDown(Keys.Escape)) Exit();

            var alt = current.IsKeyDown(Keys.LeftAlt) || current.IsKeyDown(Keys.RightAlt);
            if (alt && _last.IsKeyUp(Keys.Enter) && 
                current.IsKeyDown(Keys.Enter) && 
                _options != default)
            {
                _manager.PreferredBackBufferWidth = (int)_options.Options.Graphics.Width;
                _manager.PreferredBackBufferHeight = (int)_options.Options.Graphics.Height;
                _manager.ToggleFullScreen();
                _manager.ApplyChanges();

                _last = current;

                // Don't perform an update for the next frame to prevent the
                // keystroke from being used as normal input.
                return;
            }

            _last = current;
            base.Update(gameTime);
        }
    }
}
