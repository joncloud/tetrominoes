using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;

namespace Tetrominoes.Graphics
{
    public class GamePadFonts
    {
        public SpriteFont NormalWeight { get; }
        public GamePadFonts(FontSize size, ContentManager contentManager)
        {
            if (contentManager == default) throw new ArgumentNullException(nameof(contentManager));

            var path = Path.Combine("Fonts", "GamePad", $"{size}Normal");
            NormalWeight = contentManager.Load<SpriteFont>(path);
        }
    }
}
