#nullable disable

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using Tetrominoes.Audio;
using Tetrominoes.Input;

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
        public override void Initialize()
        {
            _input = Game.Services.GetService<IInputService>();
            _audio = Game.Services.GetService<IAudioService>();
            _menu = Game.Services.GetService<IMenuService>();
            base.Initialize();
        }

        readonly Queue<MusicTrack> _tracks = new Queue<MusicTrack>();
        public void NewMatch()
        {
            if (_match != default)
            {
                _match.Dispose();
            }

            _match = new Match(new Random(1));
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
            foreach (var track in _audio.Music.Tracks.OrderBy(_ => _match.Random.NextDouble()))
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
                00 => 0.0030f,
                01 => 0.0030f,
                02 => 0.0030f,
                03 => 0.0030f,
                04 => 0.0030f,
                05 => 0.0030f,
                06 => 0.0030f,
                07 => 0.0030f,
                08 => 0.0030f,
                09 => 0.0030f,
                10 => 0.0030f,
                11 => 0.0030f,
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

        void Score_RowsCleared(MatchScore score, int rowsCleared)
        {
            var highestRow = CalculateHighestRow(score.Match.Grid);
            _backgroundEffect.Formula.Speed = GetBackgroundSpeedForRowHeight(highestRow);
            _audio.Sound.Play(Sound.RowClear);
        }

        void Score_PieceLocked(MatchScore score, TetrominoPiece piece)
        {
            var highestRow = CalculateHighestRow(score.Match.Grid);
            _backgroundEffect.Formula.Speed = GetBackgroundSpeedForRowHeight(highestRow);
            _audio.Sound.Play(Sound.Drop);
        }

        void Score_LevelChanged(MatchScore score)
        {
            _backgroundEffect.Effect = Game.Content.Load<Effect>(
                score.Match.Random.NextElement(_effectNames)
            );
            _backgroundEffect.Formula.Frequency = score.Level switch
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
        RenderTarget2D _boardBuffer;
        SpriteBatch _spriteBatch;
        TetrominoRenderer _tetrominoRenderer;
        SpriteFont _normalWeightFont;
        SpriteFont _boldWeightFont;
        BackgroundEffect _backgroundEffect;
        const int TileWidth = 8;
        static string[] _effectNames = new[]
        {
            "Effects/Hypocycloid",
            "Effects/HorizontalCross",
            "Effects/VerticalCross",
            "Effects/Epicycloid",
            "Effects/Cycloid",
            "Effects/LissajousCurve",
            "Effects/Spirograph"
        };
        protected override void LoadContent()
        {
            _normalWeightFont = Game.Content.Load<SpriteFont>("Fonts/UI");
            _boldWeightFont = Game.Content.Load<SpriteFont>("Fonts/UI-Bold");
            _tileTexture = Game.Content.Load<Texture2D>("Textures/Tiles");
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _tetrominoRenderer = new TetrominoRenderer(
                _tileTexture,
                _spriteBatch
            );

            _boardBuffer = new RenderTarget2D(
                GraphicsDevice,
                (MatchGrid.Width + 4) * TileWidth,
                (MatchGrid.Height + 2) * TileWidth
            );


            _backgroundEffect = new BackgroundEffect(
                Game.Content.Load<Effect>(
                    _effectNames[0]
                ),
                new BackgroundEffectFormula(
                    0.2f,
                    0.5f,
                    0.001f
                )
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
            if (state.Pause == InputButtonState.Pressed)
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
                    return;
                }
            }

            if (_state != MatchState.Playing)
            {
                _backgroundEffect.Update(gameTime);
                return;
            }

            if (state.Down == InputButtonState.Pressed)
            {
                _match.Controller.Move.Down();
            }
            if (state.Left == InputButtonState.Pressed)
            {
                _match.Controller.Move.Left();
            }
            if (state.Right == InputButtonState.Pressed)
            {
                _match.Controller.Move.Right();
            }

            if (state.Swap == InputButtonState.Pressed)
            {
                _match.Controller.Swap();
            }
            if (state.RotateLeft == InputButtonState.Pressed)
            {
                _match.Controller.Rotate.Left();
            }
            if (state.RotateRight == InputButtonState.Pressed)
            {
                _match.Controller.Rotate.Right();
            }
            if (state.Drop == InputButtonState.Pressed)
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
            _backgroundEffect.Update(gameTime);

            base.Update(gameTime);
        }

        readonly string[] _pauseText = new[]
        {
            "Press Pause"
        };
        readonly string[] _lostText = new[] 
        {
            "Game Over",
            "Press Pause"
        };
        public override void Draw(GameTime gameTime)
        {
            var pp = GraphicsDevice.PresentationParameters;
            var scale = 4;
            var totalWidth = pp.BackBufferWidth / 4;
            var gridWidth = (MatchGrid.Width + 4) * TileWidth;

            RenderBackground();
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

        void RenderBackground()
        {
            var pp = GraphicsDevice.PresentationParameters;
            var scaleX = pp.BackBufferWidth / _boardBuffer.Width;
            var scaleY = pp.BackBufferHeight / _boardBuffer.Height;
            var scale = Matrix.CreateScale(scaleX, scaleX, 1);

            GraphicsDevice.SetRenderTarget(default);
            GraphicsDevice.Clear(Color.White);
            _spriteBatch.Begin(
                transformMatrix: scale,
                samplerState: SamplerState.PointWrap,
                effect: _backgroundEffect.Effect
            );
            _spriteBatch.Draw(
                _boardBuffer, 
                Vector2.Zero,
                new Color(Color.White, 0.25f)
            );
            _spriteBatch.End();
        }

        void RenderBoard()
        {
            var tx = Matrix.CreateTranslation(16, 0, 0);

            // Buffer the board to a texture to use it as a background.
            GraphicsDevice.SetRenderTarget(_boardBuffer);
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
            _spriteBatch.Draw(_boardBuffer, Vector2.Zero, Color.White);
            _spriteBatch.End();
        }

        void RenderCurrentAndShadowPieces()
        {
            var tetrominoes = _match.Tetrominoes;
            var tetrominoColor = _colors[(int)tetrominoes.Current.Piece];
            var shadowColor = new Color(tetrominoColor, 0.5f);
            _tetrominoRenderer.Draw(
                tetrominoes.Shadow,
                shadowColor
            );

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

            _spriteBatch.DrawString(
                _normalWeightFont,
                _match.Score.TotalTimeAsText,
                new Vector2(0, 0),
                Color.Black
            );
            _spriteBatch.DrawString(
                _normalWeightFont,
                "Score",
                new Vector2(0, 64),
                Color.Black
            );
            _spriteBatch.DrawString(
                _normalWeightFont,
                _match.Score.TotalScoreAsText,
                new Vector2(275, 64),
                Color.Black
            );

            _spriteBatch.DrawString(
                _normalWeightFont,
                "Level",
                new Vector2(0, 128),
                Color.Black
            );
            _spriteBatch.DrawString(
                _normalWeightFont,
                _match.Score.LevelAsText,
                new Vector2(275, 128),
                Color.Black
            );

            _spriteBatch.DrawString(
                _normalWeightFont,
                "Rows",
                new Vector2(0, 192),
                Color.Black
            );
            _spriteBatch.DrawString(
                _normalWeightFont,
                _match.Score.TotalRowsAsText,
                new Vector2(275, 192),
                Color.Black
            );

            _spriteBatch.End();
        }

        void RenderText(string[] lines, GameTime gameTime)
        {
            Span<Vector2> measurements = stackalloc Vector2[lines.Length];

            var maxWidth = 0.0f; // _boldWeightFont.MeasureString(text).X;
            for (var i = 0; i < lines.Length; i++)
            {
                var measurement = 
                    measurements[i] = 
                    _boldWeightFont.MeasureString(lines[i]);
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
                    _boldWeightFont,
                    line,
                    pos - offset,
                    Color.Black
                );
                _spriteBatch.DrawString(
                    _boldWeightFont,
                    line,
                    pos + offset,
                    Color.Black
                );
                _spriteBatch.DrawString(
                    _boldWeightFont,
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
