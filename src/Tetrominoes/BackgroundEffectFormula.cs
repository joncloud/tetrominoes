using Microsoft.Xna.Framework.Graphics;
using System;

namespace Tetrominoes
{
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
