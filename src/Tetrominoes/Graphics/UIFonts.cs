using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;

namespace Tetrominoes.Graphics
{
    public class UIFonts
    {
        public SpriteFont NormalWeight { get; }
        public SpriteFont BoldWeight { get; }

        public UIFonts(FontSize size, ContentManager contentManager)
        {
            if (contentManager == default) throw new ArgumentNullException(nameof(contentManager));

            NormalWeight = contentManager.Load<SpriteFont>(
                Path.Combine("Fonts", "UI", $"{size}Normal")
            );
            BoldWeight = contentManager.Load<SpriteFont>(
                Path.Combine("Fonts", "UI", $"{size}Bold")
            );
        }
    }
}
