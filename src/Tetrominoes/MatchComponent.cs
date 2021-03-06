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
using Tetrominoes.Options;

namespace Tetrominoes
{
    public class MatchComponent : DrawableGameComponent, IMatchService
    {
        public MatchComponent(Game game) : base(game)
        {
            Visible = Enabled = false;
        }

        public static MatchComponent AddTo(Game game)
        {
            var component = new MatchComponent(game);
            game.Components.Add(component);
            game.Services.AddService<IMatchService>(component);
            return component;
        }

        Match _match;
        IInputService _input;
        IAudioService _audio;
        IMenuService _menu;
        IBackgroundService _background;
        IOptionService _option;
        public override void Initialize()
        {
            _input = Game.Services.GetService<IInputService>();
            _audio = Game.Services.GetService<IAudioService>();
            _menu = Game.Services.GetService<IMenuService>();
            _background = Game.Services.GetService<IBackgroundService>();
            _option = Game.Services.GetService<IOptionService>();

            base.Initialize();
        }

        readonly Queue<MusicTrack> _tracks = new Queue<MusicTrack>();
        public void NewMatch()
        {
            if (_match != default)
            {
                _match.Dispose();
            }

            _match = new Match(
#if DEBUG
                new Random(1),
#else
                new Random(),
#endif
                _option
            );
            _match.Score.LevelChanged += Score_LevelChanged;
            _match.Score.PieceLocked += Score_PieceLocked;
            _match.Score.RowsCleared += Score_RowsCleared;
            _match.Score.PieceMoved += Score_PieceMoved;
            _match.Score.PieceRotated += Score_PieceRotated;
            _match.Score.PieceSwapped += Score_PieceSwapped;
            Visible = Enabled = true;

            PlayNextTrack();
        }

        void Score_PieceSwapped(MatchScore _)
        {
            _audio.Sound.Play(Sound.Move);
        }

        void SetupPlaylist()
        {
            _tracks.Clear();
            foreach (var track in _audio.Music.Tracks.OrderBy(_ => Guid.NewGuid()))
            {
                _tracks.Enqueue(track);
            }
        }

        void PlayNextTrack()
        {
            if (!_tracks.TryDequeue(out var track))
            {
                SetupPlaylist();
                track = _tracks.Dequeue();
            }

            _audio.Music.Play(track);
        }

        void Score_PieceRotated(MatchScore _, RotationDirection direction)
        {
            var sound = direction switch
            {
                RotationDirection.Left => Sound.RotateLeft,
                RotationDirection.Right => Sound.RotateRight,
                _ => Sound.RotateLeft
            };
            _audio.Sound.Play(sound);
        }

        void Score_PieceMoved(MatchScore _, MovementType type)
        {
            if (type == MovementType.Automatic) return;
            _audio.Sound.Play(Sound.Move);
        }

        static int CalculateHighestRow(MatchGrid grid)
        {
            for (var i = 0; i < MatchGrid.Size; i++)
            {
                var piece = grid[i];
                if (piece != TetrominoPiece.Empty)
                {
                    var (_, y) = MatchGrid.GetPosition(i);
                    return y;
                }
            }
            return MatchGrid.Height;
        }

        static float GetBackgroundSpeedForRowHeight(int rowHeight)
        {
            return rowHeight switch
            {
                00 => 0.0070f,
                01 => 0.0065f,
                02 => 0.0060f,
                03 => 0.0055f,
                04 => 0.0050f,
                05 => 0.0045f,
                06 => 0.0040f,
                07 => 0.0035f,
                08 => 0.0034f,
                09 => 0.0033f,
                10 => 0.0032f,
                11 => 0.0031f,
                12 => 0.0030f,
                13 => 0.0029f,

                14 => 0.0028f,
                15 => 0.0027f,
                16 => 0.0026f,
                17 => 0.0025f,
                18 => 0.0024f,
                19 => 0.0023f,
                20 => 0.0022f,
                21 => 0.0021f,
                22 => 0.0020f,
                23 => 0.0019f,
                24 => 0.0018f,

                25 => 0.0017f,
                26 => 0.0016f,
                27 => 0.0015f,
                28 => 0.0014f,
                29 => 0.0013f,
                30 => 0.0012f,
                31 => 0.0011f,
                32 => 0.0010f,
                _ => 0.001f
            };
        }

        static float GetMusicPitchForRowHeight(int rowHeight)
        {
            // As row height gets closer to the top (0)
            // rapidly increase the y value.
            var x = 1 - rowHeight / 32.0f;

            // (2^(2x-2))*2
            var y = (float)(Math.Pow(2, (2 * x) - 2) * x);

            // Make sure to clamp to prevent an exception when going beyond 1.0f.
            return MathHelper.Clamp(y, 0.0f, 1.0f);
        }

        void Score_RowsCleared(MatchScore score, int rowsCleared)
        {
            var highestRow = CalculateHighestRow(score.Match.Grid);
            _background.BackgroundEffect.Formula.Speed = GetBackgroundSpeedForRowHeight(highestRow);
            _audio.Sound.Play(Sound.RowClear);
            _audio.Music.CurrentTrack?.SetPitch(
                GetMusicPitchForRowHeight(highestRow)
            );
        }

        void Score_PieceLocked(MatchScore score, TetrominoPiece piece)
        {
            var highestRow = CalculateHighestRow(score.Match.Grid);
            _background.BackgroundEffect.Formula.Speed = GetBackgroundSpeedForRowHeight(highestRow);
            _audio.Sound.Play(Sound.Drop);
            _audio.Music.CurrentTrack?.SetPitch(
                GetMusicPitchForRowHeight(highestRow)
            );
        }

        void Score_LevelChanged(MatchScore score)
        {
            _background.NextEffect();
            _background.BackgroundEffect.Formula.Frequency = score.Level switch
            {
                1 => 0.5f,
                2 => 0.55f,
                3 => 0.60f,
                4 => 0.75f,
                5 => 0.80f,
                6 => 0.90f,
                8 => 1.00f,
                9 => 1.10f,
                _ => 1.25f
            };

            PlayNextTrack();
        }

        Texture2D _tileTexture;
        RenderTarget2D _backgroundTexture;
        RenderTarget2D _previewTexture;
        SpriteBatch _spriteBatch;
        TetrominoRenderer _tetrominoRenderer;
        UIFonts _largeUIFonts;
        UIFonts _smallUIFonts;
        const int TileWidth = 8;
        protected override void LoadContent()
        {
            _largeUIFonts = new UIFonts(FontSize.Large, Game.Content);
            _smallUIFonts = new UIFonts(FontSize.Small, Game.Content);
            _tileTexture = Game.Content.Load<Texture2D>("Textures/Tiles");
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _tetrominoRenderer = new TetrominoRenderer(
                _tileTexture,
                _spriteBatch
            );

            CreatePreviewTexture();
            CreateBackgroundTexture();

            base.LoadContent();
        }

        void CreatePreviewTexture()
        {
            _previewTexture = new RenderTarget2D(
                GraphicsDevice,
                6 * TileWidth,
                6 * TileWidth
            );
            GraphicsDevice.SetRenderTarget(_previewTexture);
            _spriteBatch.Begin(samplerState: SamplerState.PointWrap);

            // TODO This could probably be done with one call
            for (var x = 0; x < 8; x++)
            {
                for (var y = 0; y < 8; y++)
                {
                    _spriteBatch.Draw(
                        _tileTexture,
                        new Vector2(x, y) * TileWidth,
                        new Rectangle(0, 0, TileWidth, TileWidth),
                        Color.White
                    );
                }
            }
            _spriteBatch.End();
            GraphicsDevice.SetRenderTarget(default);
        }

        void CreateBackgroundTexture()
        {
            _backgroundTexture = new RenderTarget2D(
                GraphicsDevice,
                (MatchGrid.Width + 4) * TileWidth,
                (MatchGrid.Height + 2) * TileWidth
            );
            GraphicsDevice.SetRenderTarget(_backgroundTexture);
            _spriteBatch.Begin(samplerState: SamplerState.PointWrap);

            // TODO This could probably be done with one call
            for (var x = 2; x < MatchGrid.Width + 2; x++)
            {
                for (var y = 2; y < MatchGrid.Height; y++)
                {
                    _spriteBatch.Draw(
                        _tileTexture,
                        new Vector2(x, y) * TileWidth,
                        new Rectangle(0, 0, TileWidth, TileWidth),
                        Color.White
                    );
                }
            }
            _spriteBatch.End();
            GraphicsDevice.SetRenderTarget(default);
        }

        static readonly Color[] _colors = new[]
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

#if DEBUG
        KeyboardState _last;
        KeyboardState _current;
        bool IsPress(Keys k) =>
            _last.IsKeyUp(k) && _current.IsKeyDown(k);

        void HandleKeyboard()
        {
            _current = Keyboard.GetState();
            if (IsPress(Keys.Enter))
            {
                _match.Score.AddRowsCleared(10);
            }

            _last = _current;
        }
#endif

        MatchState _state;
        public override void Update(GameTime gameTime)
        {
#if DEBUG
            HandleKeyboard();
#endif

            var state = _input.State;
            if (_state != MatchState.Playing &&
                state.Back.State == InputButtonState.Pressed)
            {
                Enabled = Visible = false;
                _menu.Show();
                _state = MatchState.Playing;
                _audio.Music.Pause();
                return;
            }

            if (state.Pause.State == InputButtonState.Pressed)
            {
                if (_state == MatchState.Paused)
                {
                    _state = MatchState.Playing;
                    _audio.Music.Resume();
                }
                else if (_state == MatchState.Playing)
                {
                    _state = MatchState.Paused;
                    _audio.Music.Pause();
                }
                else if (_state == MatchState.Lost)
                {
                    Enabled = Visible = false;
                    _menu.Show();
                    _state = MatchState.Playing;
                    _audio.Music.Pause();
                    return;
                }
            }

            if (_state != MatchState.Playing)
            {
                return;
            }

            if (state.Down.State == InputButtonState.Pressed ||
                (_option.Options.Gameplay.SlideSpeed != SlideSpeed.Off &&
                state.Down.IsHeldForInterval((int)_option.Options.Gameplay.SlideSpeed)))
            {
                _match.Controller.Move.Down();
            }
            if (state.Left.State == InputButtonState.Pressed ||
                (_option.Options.Gameplay.SlideSpeed != SlideSpeed.Off &&
                state.Left.IsHeldForInterval((int)_option.Options.Gameplay.SlideSpeed)))
            {
                _match.Controller.Move.Left();
            }
            if (state.Right.State == InputButtonState.Pressed ||
                (_option.Options.Gameplay.SlideSpeed != SlideSpeed.Off &&
                state.Right.IsHeldForInterval((int)_option.Options.Gameplay.SlideSpeed)))
            {
                _match.Controller.Move.Right();
            }

            if (state.Swap.State == InputButtonState.Pressed)
            {
                _match.Controller.Swap();
            }
            if (state.RotateLeft.State == InputButtonState.Pressed)
            {
                _match.Controller.Rotate.Left();
            }
            if (state.RotateRight.State == InputButtonState.Pressed)
            {
                _match.Controller.Rotate.Right();
            }
            if (state.Drop.State == InputButtonState.Pressed)
            {
                _match.Tetrominoes.Current.Position =
                    _match.Tetrominoes.Shadow.Position;
                _match.Controller.Move.Down();
            }

            _state = _match.Update(gameTime);
            if (_state == MatchState.Lost)
            {
                _audio.Music.Pause();
            }

            base.Update(gameTime);
        }

        readonly string[] _pauseText = new[]
        {
            "Pause to Resume",
            "Back to Leave"
        };
        readonly string[] _lostText = new[] 
        {
            "Game Over",
            "Pause to Leave"
        };
        public override void Draw(GameTime gameTime)
        {
            var pp = GraphicsDevice.PresentationParameters;
            var scale = 4;
            var totalWidth = pp.BackBufferWidth / 4;
            var gridWidth = (MatchGrid.Width + 4) * TileWidth;

            RenderBoard();
            RenderBuffer(scale, totalWidth, gridWidth);
            RenderHud(scale, totalWidth, gridWidth);
            if (_state == MatchState.Paused)
            {
                RenderText(_pauseText, gameTime);
            }
            else if(_state == MatchState.Lost)
            {
                RenderText(_lostText, gameTime);
            }

            base.Draw(gameTime);
        }

        void RenderBoard()
        {
            var tx = Matrix.CreateTranslation(16, 0, 0);

            // Buffer the board to a texture to use it as a background.
            GraphicsDevice.SetRenderTarget(_background.Board);
            GraphicsDevice.Clear(Color.White);
            _spriteBatch.Begin(transformMatrix: tx, samplerState: SamplerState.PointClamp);

            _spriteBatch.Draw(
                _backgroundTexture,
                new Vector2(-16, 0),
                new Color(Color.White, 0.10f)
            );

            var grid = _match.Grid;
            for (var i = 0; i < MatchGrid.Size; i++)
            {
                var type = grid[i];
                if (type == TetrominoPiece.Empty) continue;

                var color = _colors[(int)type];

                var point = MatchGrid.GetPosition(i).ToVector2() * TileWidth;
                _spriteBatch.Draw(
                    _tileTexture,
                    point,
                    new Rectangle(0, 0, TileWidth, TileWidth),
                    color
                );
            }

            RenderCurrentAndShadowPieces();

            _spriteBatch.End();
        }

        void RenderBuffer(int scale, int totalWidth, int gridWidth)
        {
            var tx = Matrix.CreateTranslation((totalWidth - gridWidth) / 2, 0, 0)
               * Matrix.CreateScale(scale, scale, 0);

            // Draw the buffer to the screen.
            GraphicsDevice.SetRenderTarget(default);
            _spriteBatch.Begin(transformMatrix: tx, samplerState: SamplerState.PointClamp);
            _spriteBatch.Draw(_background.Board, Vector2.Zero, Color.White);
            _spriteBatch.End();
        }

        void RenderCurrentAndShadowPieces()
        {
            var tetrominoes = _match.Tetrominoes;
            var tetrominoColor = _colors[(int)tetrominoes.Current.Piece];

            if (_option.Options.Gameplay.ShadowPiece)
            {
                var shadowColor = new Color(tetrominoColor, 0.5f);
                _tetrominoRenderer.Draw(
                    tetrominoes.Shadow,
                    shadowColor
                );
            }

            _tetrominoRenderer.Draw(
                tetrominoes.Current,
                tetrominoColor
            );
        }

        void RenderHud(int scale, int totalWidth, int gridWidth)
        {
            RenderNextPiece(scale, totalWidth, gridWidth);
            RenderSwapPiece(scale, totalWidth, gridWidth);
            RenderScore();
        }

        void RenderNextPiece(int scale, int totalWidth, int gridWidth)
        {
            var tx = Matrix.CreateTranslation((totalWidth - gridWidth) / 2, 0, 0)
                * Matrix.CreateTranslation(gridWidth + 48, 64, 0)
                * Matrix.CreateScale(scale, scale, 0);

            _spriteBatch.Begin(transformMatrix: tx, samplerState: SamplerState.PointClamp);

            _spriteBatch.Draw(
                _previewTexture,
                -Vector2.One * 16,
                new Color(Color.White, 0.10f)
            );

            var tetrominoes = _match.Tetrominoes;
            _tetrominoRenderer.Draw(
                tetrominoes.Next,
                _colors[(int)tetrominoes.Next.Piece]
            );

            _spriteBatch.End();
        }

        void RenderSwapPiece(int scale, int totalWidth, int gridWidth)
        {
            if (!_option.Options.Gameplay.SwapPiece) return;

            var tx = Matrix.CreateTranslation((totalWidth - gridWidth) / 2, 0, 0)
                * Matrix.CreateTranslation(gridWidth + 48, 128, 0)
                * Matrix.CreateScale(scale, scale, 0);

            _spriteBatch.Begin(transformMatrix: tx, samplerState: SamplerState.PointClamp);

            _spriteBatch.Draw(
                _previewTexture,
                -Vector2.One * 16,
                new Color(Color.White, 0.10f)
            );

            var tetrominoes = _match.Tetrominoes;
            if (tetrominoes.Swap != default)
            {
                _tetrominoRenderer.Draw(
                    tetrominoes.Swap,
                    _colors[(int)tetrominoes.Swap.Piece]
                );
            }

            _spriteBatch.End();
        }

        void RenderScore()
        {
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            var pos = new Vector2(114, 0);
            _spriteBatch.DrawString(
                _largeUIFonts.NormalWeight,
                _match.Score.TotalTimeAsText,
                pos,
                Color.Black
            );

            pos.X += _largeUIFonts.NormalWeight
                .MeasureString(_match.Score.TotalTimeAsText)
                .X;

            _spriteBatch.DrawString(
                _smallUIFonts.NormalWeight,
                _match.Score.FractionalSecondsAsText,
                pos,
                Color.Black
            );

            _spriteBatch.DrawString(
                _largeUIFonts.NormalWeight,
                "Score",
                new Vector2(0, 64),
                Color.Black
            );
            _spriteBatch.DrawString(
                _largeUIFonts.NormalWeight,
                _match.Score.TotalScoreAsText,
                new Vector2(211, 64),
                Color.Black
            );

            _spriteBatch.DrawString(
                _largeUIFonts.NormalWeight,
                "Level",
                new Vector2(0, 128),
                Color.Black
            );
            _spriteBatch.DrawString(
                _largeUIFonts.NormalWeight,
                _match.Score.LevelAsText,
                new Vector2(211, 128),
                Color.Black
            );

            _spriteBatch.DrawString(
                _largeUIFonts.NormalWeight,
                "Rows",
                new Vector2(0, 192),
                Color.Black
            );
            _spriteBatch.DrawString(
                _largeUIFonts.NormalWeight,
                _match.Score.TotalRowsAsText,
                new Vector2(211, 192),
                Color.Black
            );

            _spriteBatch.End();
        }

        void RenderText(string[] lines, GameTime gameTime)
        {
            Span<Vector2> measurements = stackalloc Vector2[lines.Length];

            var maxWidth = 0.0f;
            for (var i = 0; i < lines.Length; i++)
            {
                var measurement = 
                    measurements[i] = 
                    _largeUIFonts.BoldWeight.MeasureString(lines[i]);
                maxWidth = Math.Max(maxWidth, measurement.X);
            }

            var pp = GraphicsDevice.PresentationParameters;
            var tx = Matrix.CreateTranslation(
                (pp.BackBufferWidth - maxWidth) / 2,
                (pp.BackBufferHeight - 64) / 2,
                0
            );

            _spriteBatch.Begin(transformMatrix: tx, samplerState: SamplerState.PointClamp);

            // Vary the border to make it look on purpose
            var x = 
                (float)(3 * Math.Sin(gameTime.TotalGameTime.TotalMilliseconds / 200)) + 5f;
            var y =
                (float)(3 * Math.Cos(gameTime.TotalGameTime.TotalMilliseconds / 200)) + 5f;
            var offset = new Vector2(x, y);

            var pos = Vector2.Zero;
            for (var i = 0; i < lines.Length; i++)
            {
                var measurement = measurements[i];
                var line = lines[i];

                pos.X = (maxWidth - measurement.X) / 2;

                // Poor implementation for borders
                _spriteBatch.DrawString(
                    _largeUIFonts.BoldWeight,
                    line,
                    pos - offset,
                    Color.Black
                );
                _spriteBatch.DrawString(
                    _largeUIFonts.BoldWeight,
                    line,
                    pos + offset,
                    Color.Black
                );
                _spriteBatch.DrawString(
                    _largeUIFonts.BoldWeight,
                    line,
                    pos,
                    Color.White
                );

                pos.Y += measurement.Y;
            }

            _spriteBatch.End();
        }
    }
}
