using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Tetrominoes
{
    public class BackgroundEffect
    {
        public static IReadOnlyList<string> EffectNames { get; } = new[]
        {
            "Effects/Hypocycloid",
            "Effects/HorizontalCross",
            "Effects/VerticalCross",
            "Effects/Epicycloid",
            "Effects/Cycloid",
            "Effects/LissajousCurve",
            "Effects/Spirograph"
        };
        Effect _effect;
        public Effect Effect
        {
            get => _effect;
            set
            {
                _effect = value ?? throw new ArgumentNullException(nameof(value));
            }
        }
        public BackgroundEffectFormula Formula { get; }
        public BackgroundEffect(Effect effect, BackgroundEffectFormula formula)
        {
            _effect = effect ?? throw new ArgumentNullException(nameof(effect));
            Formula = formula ?? throw new ArgumentNullException(nameof(formula));
        }

        public void Update(GameTime gameTime)
        {
            Effect.Parameters["t"].SetValue(
                (float)gameTime.TotalGameTime.TotalMilliseconds
            );
            Formula.Apply(Effect);
        }
    }
}
