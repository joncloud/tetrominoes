using Nett;
using System;

namespace Tetrominoes.Options
{
    public class GameInputOptions
    {
        public GameInputOptions()
        {
            Keyboard = new GameInputKeyboardOptions();
            GamePad = new GameInputGamePadOptions();
        }

        public GameInputOptions(
            GameInputKeyboardOptions keyboard,
            GameInputGamePadOptions gamePad
        )
        {
            Keyboard = keyboard ?? throw new ArgumentNullException(nameof(keyboard));
            GamePad = gamePad ?? throw new ArgumentNullException(nameof(gamePad));
        }

        [TomlMember(Key = "keyboard")]
        public GameInputKeyboardOptions Keyboard { get; }

        [TomlMember(Key = "gamepad")]
        public GameInputGamePadOptions GamePad { get; }

        public static GameInputOptions FromToml(TomlTable table)
        {
            if (table == default) throw new ArgumentNullException(nameof(table));

            var options = new GameInputOptions(
                GameInputKeyboardOptions.FromToml(table),
                GameInputGamePadOptions.FromToml(table)
            );

            return options;
        }
    }
}
