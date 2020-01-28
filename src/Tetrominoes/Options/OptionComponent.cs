#nullable disable

using Microsoft.Xna.Framework;
using System;

namespace Tetrominoes.Options
{
    public class OptionComponent : GameComponent, IOptionService
    {
        public OptionComponent(Game game)
            : base(game)
        {
            Load();
        }

        public event Action<GameOptions> Updated;

        public GameOptions Options { get; private set; }

        public static OptionComponent AddTo(Game game)
        {
            var component = new OptionComponent(game);
            game.Components.Add(component);
            game.Services.AddService<IOptionService>(component);
            return component;
        }

        void Load()
        {
            Options = GameOptions.FromConfigOrDefault();
            Updated?.Invoke(Options);
        }

        public void Reload()
        {
            Load();
        }

        public void Save()
        {
            Options.Save();
            Updated?.Invoke(Options);
        }
    }
}
