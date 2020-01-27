using Nett;

namespace Tetrominoes.Options
{
    public class GameInputOptions
    {
        public static GameInputOptions CreateDefault() =>
            new GameInputOptions
            {
                Keyboard = GameInputKeyboardOptions.CreateDefault()
            };

        [TomlMember(Key = "keyboard")]
        public GameInputKeyboardOptions Keyboard { get; set; }

        [TomlMember(Key = "gamepad")]
        public GameInputGamePadOptions GamePad { get; set; }

        internal void ValidateConstraints()
        {
            if (Keyboard == null)
            {
                Keyboard = GameInputKeyboardOptions.CreateDefault();
            }
            else
            {
                Keyboard.ValidateConstraints();
            }

            if (GamePad == null)
            {
                GamePad = GameInputGamePadOptions.CreateDefault();
            }
            else
            {
                GamePad.ValidateConstraints();
            }
        }
    }
}
