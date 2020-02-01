using Nett;
using System;

namespace Tetrominoes.Options
{
    public class GameGameplayOptions
    {
        public GameGameplayOptions()
        {
            ShadowPiece = true;
            SwapPiece = true;
            SlideSpeed = SlideSpeed.Slow;
        }

        public static GameGameplayOptions FromToml(TomlTable table)
        {
            if (table == default) throw new ArgumentNullException(nameof(table));

            var options = new GameGameplayOptions();

            if (table.TryGetTable("gameplay", out var graphics))
            {
                if (graphics.TryGetValue("shadow_piece", out bool shadowPiece))
                {
                    options.ShadowPiece = shadowPiece;
                }
                if (graphics.TryGetValue("swap_piece", out bool swapPiece))
                {
                    options.SwapPiece = swapPiece;
                }
                if (graphics.TryGetEnumValue<SlideSpeed>("slide_speed", out var slideSpeed))
                {
                    options.SlideSpeed = slideSpeed;
                }
            }

            return options;
        }

        [TomlMember(Key = "shadow_piece")]
        public bool ShadowPiece { get; set; }

        [TomlMember(Key = "swap_piece")]
        public bool SwapPiece { get; set; }

        [TomlMember(Key = "slide_speed")]
        public SlideSpeed SlideSpeed { get; set; }
    }
}
