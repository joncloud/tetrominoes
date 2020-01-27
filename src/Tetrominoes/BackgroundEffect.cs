using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Tetrominoes
{
    public class BackgroundEffect
    {
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
            Effect = effect ?? throw new ArgumentNullException(nameof(effect));
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


    public class BackgroundEffectFormula
    {
        public float Amplitude;
        public float Frequency;
        public float Speed;
        public BackgroundEffectFormula(float amplitude, float frequency, float speed)
        {
            Amplitude = amplitude;
            Frequency = frequency;
            Speed = speed;
        }

        public void Apply(Effect effect)
        {
            effect.Parameters["A"].SetValue(Amplitude);
            effect.Parameters["F"].SetValue(Frequency);
            effect.Parameters["S"].SetValue(Speed);
        }

        public float Calculate(float y, float t) =>
            (float)(Amplitude * Math.Sin((Frequency * y) + (Speed * t)));

        public override string ToString() =>
            $"A{Amplitude},F{Frequency},S{Speed}";
    }
}
