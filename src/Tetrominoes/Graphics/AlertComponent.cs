#nullable disable
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetrominoes.Graphics
{
    public class AlertComponent : DrawableGameComponent, IAlertService
    {
        public AlertComponent(Game game) : base(game)
        {
            Enabled = Visible = false;
        }

        public static AlertComponent AddTo(Game game)
        {
            var component = new AlertComponent(game);
            game.Components.Add(component);
            game.Services.AddService<IAlertService>(component);
            return component;
        }

        UIFonts _uiFonts;
        SpriteBatch _spriteBatch;
        protected override void LoadContent()
        {
            _uiFonts = new UIFonts(FontSize.Small, Game.Content);
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            base.LoadContent();
        }

        readonly Queue<Message> _messages = new Queue<Message>();
        Message _current;

        public override void Update(GameTime gameTime)
        {
            if (_current == default)
            {
                if (!_messages.TryDequeue(out _current))
                {
                    Enabled = Visible = false;
                }
            }
            else
            {
                if (_current.TryUpdate(gameTime))
                {
                    _current = default;
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (_current == default) return;

            var pp = GraphicsDevice.PresentationParameters;
            var tx = Matrix.CreateTranslation(
                pp.BackBufferWidth,
                pp.BackBufferHeight,
                0
            );
            _spriteBatch.Begin(transformMatrix: tx, samplerState: SamplerState.PointClamp);
            _current.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public void Display(string text)
        {
            _messages.Enqueue(
                new Message(
                    text,
                    _uiFonts.BoldWeight
                )
            );
            Enabled = Visible = true;
        }

        class Message
        {
            readonly string _source;
            readonly StringBuilder _stringBuilder;
            readonly SpriteFont _font;
            int _position;
            public Message(string text, SpriteFont font)
            {
                _source = text ?? throw new ArgumentNullException(nameof(text));
                _font = font ?? throw new ArgumentOutOfRangeException(nameof(font));
                _stringBuilder = new StringBuilder(text.Length);
            }

            TimeSpan _timer;
            int _target = 50;
            public bool TryUpdate(GameTime gameTime)
            {
                _timer += gameTime.ElapsedGameTime;
                while (_timer.TotalMilliseconds > _target)
                {
                    _timer -= TimeSpan.FromMilliseconds(_target);

                    if (_position < _source.Length)
                    {
                        _stringBuilder.Append(_source[_position]);
                        _position++;
                    }
                    else if (_position == _source.Length)
                    {
                        _target = 2000;
                        _position++;
                    }
                    else if (_position > _source.Length)
                    {
                        return true;
                    }
                }

                return false;
            }

            public void Draw(SpriteBatch batch)
            {
                var measurement = _font.MeasureString(_stringBuilder);
                batch.DrawString(
                    _font,
                    _stringBuilder,
                    -measurement,
                    Color.Black
                );
            }
        }
    }
}
