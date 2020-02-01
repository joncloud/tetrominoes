#nullable disable

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetrominoes
{
    public class BackgroundComponent : DrawableGameComponent, IBackgroundService
    {
        public BackgroundComponent(Game game) : base(game)
        {
            DrawOrder = -1;
        }

        public static BackgroundComponent AddTo(Game game)
        {
            var component = new BackgroundComponent(game);
            game.Components.Add(component);
            game.Services.AddService<IBackgroundService>(component);
            return component;
        }

        public RenderTarget2D Board { get; private set; }
        public BackgroundEffect BackgroundEffect { get; private set; }

        SpriteBatch _spriteBatch;
#if DEBUG
        readonly Random _random = new Random(0);
#else
        readonly Random _random = new Random();
#endif

        public void Clear()
        {
            var targets = GraphicsDevice.GetRenderTargets();
            GraphicsDevice.SetRenderTarget(Board);
            GraphicsDevice.Clear(Color.Transparent);
            GraphicsDevice.SetRenderTargets(targets);
        }

        public void NextEffect()
        {
            BackgroundEffect.Effect = Game.Content.Load<Effect>(
                _random.NextElement(BackgroundEffect.EffectNames)
            );
        }

        protected override void LoadContent()
        {
            Board = new RenderTarget2D(
                GraphicsDevice,
                (MatchGrid.Width + 4) * 8,
                (MatchGrid.Height + 2) * 8
            );
            BackgroundEffect = new BackgroundEffect(
                Game.Content.Load<Effect>(
                    BackgroundEffect.EffectNames[0]
                ),
                new BackgroundEffectFormula(
                    0.2f,
                    0.5f,
                    0.001f
                )
            );

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            BackgroundEffect.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            var pp = GraphicsDevice.PresentationParameters;
            var scaleX = pp.BackBufferWidth / Board.Width;
            var scaleY = pp.BackBufferHeight / Board.Height;
            var scale = Matrix.CreateScale(scaleX, scaleX, 1);

            GraphicsDevice.SetRenderTarget(default);
            GraphicsDevice.Clear(Color.White);
            _spriteBatch.Begin(
                transformMatrix: scale,
                samplerState: SamplerState.PointWrap,
                effect: BackgroundEffect.Effect
            );
            _spriteBatch.Draw(
                Board,
                Vector2.Zero,
                new Color(Color.White, 0.25f)
            );
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
