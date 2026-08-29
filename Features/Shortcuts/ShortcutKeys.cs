using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Windows.Configurations.Features.Shortcuts
{
    internal static class ShortcutKeys
    {
        public static string Format(Keys keyData)
        {
            List<string> parts = [];

            if ((keyData & Keys.Control) == Keys.Control)
                parts.Add("Ctrl");

            if ((keyData & Keys.Alt) == Keys.Alt)
                parts.Add("Alt");

            if ((keyData & Keys.Shift) == Keys.Shift)
                parts.Add("Shift");

            Keys key = keyData & Keys.KeyCode;

            if (!IsModifier(key))
                parts.Add(KeyName(key));

            return string.Join(" + ", parts);
        }

        /// <summary>
        /// Uma combinação só está completa quando há uma tecla além dos modificadores.
        /// </summary>
        public static bool IsComplete(Keys keyData)
        {
            Keys key = keyData & Keys.KeyCode;

            return key != Keys.None && !IsModifier(key);
        }

        public static bool TryParse(string shortcut, out Keys keyData)
        {
            keyData = Keys.None;

            if (string.IsNullOrWhiteSpace(shortcut))
                return false;

            foreach (string part in shortcut.Split(" + ", StringSplitOptions.RemoveEmptyEntries))
            {
                switch (part)
                {
                    case "Ctrl":
                        keyData |= Keys.Control;
                        continue;
                    case "Alt":
                        keyData |= Keys.Alt;
                        continue;
                    case "Shift":
                        keyData |= Keys.Shift;
                        continue;
                }

                if (!TryParseKey(part, out Keys key))
                    return false;

                keyData |= key;
            }

            return IsComplete(keyData);
        }

        private static bool TryParseKey(string name, out Keys key)
        {
            if (name.Length == 1 && name[0] >= '0' && name[0] <= '9')
            {
                key = Keys.D0 + (name[0] - '0');
                return true;
            }

            if (name.StartsWith("Num ") && name.Length == 5 && name[4] >= '0' && name[4] <= '9')
            {
                key = Keys.NumPad0 + (name[4] - '0');
                return true;
            }

            key = name switch
            {
                "+" => Keys.Oemplus,
                "-" => Keys.OemMinus,
                "," => Keys.Oemcomma,
                "." => Keys.OemPeriod,
                "Page Up" => Keys.Prior,
                "Page Down" => Keys.Next,
                "Enter" => Keys.Return,
                "Caps Lock" => Keys.Capital,
                _ => Enum.TryParse(name, out Keys parsed) ? parsed : Keys.None
            };

            return key != Keys.None;
        }

        private static bool IsModifier(Keys key)
        {
            return key is Keys.ControlKey or Keys.Menu or Keys.ShiftKey or Keys.LWin or Keys.RWin;
        }

        private static string KeyName(Keys key)
        {
            if (key >= Keys.D0 && key <= Keys.D9)
                return ((char)('0' + (key - Keys.D0))).ToString();

            if (key >= Keys.NumPad0 && key <= Keys.NumPad9)
                return $"Num {key - Keys.NumPad0}";

            return key switch
            {
                Keys.Oemplus => "+",
                Keys.OemMinus => "-",
                Keys.Oemcomma => ",",
                Keys.OemPeriod => ".",
                Keys.Prior => "Page Up",
                Keys.Next => "Page Down",
                Keys.Return => "Enter",
                Keys.Capital => "Caps Lock",
                _ => key.ToString()
            };
        }
    }
}
