using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Windows.Configurations.Features.Shortcuts
{
    /// <summary>
    /// Janela apenas de mensagens: mantém os atalhos globais válidos mesmo quando o
    /// handle do formulário é recriado (ex.: ao alternar o ShowInTaskbar).
    /// </summary>
    internal sealed class HotkeyManager : NativeWindow, IDisposable
    {
        private const int WM_HOTKEY = 0x0312;
        private const uint MOD_ALT = 0x0001;
        private const uint MOD_CONTROL = 0x0002;
        private const uint MOD_SHIFT = 0x0004;
        private const uint MOD_NOREPEAT = 0x4000;

        private readonly Dictionary<int, Action> _actions = [];
        private int _nextId;

        public HotkeyManager()
        {
            CreateHandle(new CreateParams());
        }

        public bool Register(Keys keyData, Action action)
        {
            uint key = (uint)(keyData & Keys.KeyCode);

            if (key == 0)
                return false;

            uint modifiers = MOD_NOREPEAT;

            if ((keyData & Keys.Control) == Keys.Control)
                modifiers |= MOD_CONTROL;

            if ((keyData & Keys.Alt) == Keys.Alt)
                modifiers |= MOD_ALT;

            if ((keyData & Keys.Shift) == Keys.Shift)
                modifiers |= MOD_SHIFT;

            int id = ++_nextId;

            if (!RegisterHotKey(Handle, id, modifiers, key))
                return false;

            _actions[id] = action;

            return true;
        }

        public void Clear()
        {
            foreach (int id in _actions.Keys)
                UnregisterHotKey(Handle, id);

            _actions.Clear();
        }

        public void Dispose()
        {
            Clear();
            DestroyHandle();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HOTKEY && _actions.TryGetValue((int)m.WParam, out Action action))
                action();

            base.WndProc(ref m);
        }

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
    }
}
