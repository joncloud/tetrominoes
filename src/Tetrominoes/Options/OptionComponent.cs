using Microsoft.Xna.Framework;

namespace Tetrominoes.Options
{
    public class OptionComponent : GameComponent, IOptionService
    {
        public OptionComponent(Game game)
            : base(game)
        {
            Load();
        }

        public GameOptions Options { get; private set; }

        public static OptionComponent AddTo(Game game)
        {
            var component = new OptionComponent(game);
            game.Components.Add(component);
            game.Services.AddService<IOptionService>(component);
            return component;
        }

        //IMessageService _messageService;
        //IBackgroundService _backgroundService;
        public override void Initialize()
        {
            //_messageService = Game.Services.GetService<IMessageService>();
            //_backgroundService = Game.Services.GetService<IBackgroundService>();

            base.Initialize();
        }

        void Load()
        {
            Options = GameOptions.FromConfigOrDefault();
        }

        public void Reload()
        {
            //_backgroundService.QueueWork(Load);
        }

        public void Save()
        {
            //_backgroundService.QueueWork(Options.Save);

            //var msg = ObjectPool<OptionsUpdated>.Default.Rent();
            //_messageService.Handle(msg);
        }
    }
}
