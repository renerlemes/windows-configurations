using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Windows.Configurations.Configuration;
using Windows.Configurations.Configuration.Audio;
using Windows.Configurations.Features.Audio;
using Windows.Configurations.Features.Shortcuts;
using Windows.Configurations.Features.Startup;

namespace Windows.Configurations
{
    public partial class frmDefault : Form
    {
        private readonly MuteOnLockMonitor _muteOnLockMonitor = new();
        private readonly HotkeyManager _hotkeys = new();
        private AppConfiguration _settings;
        private bool _allowVisible;
        private Font _trayHeaderFont;
        private Icon _defaultTrayIcon;
        private Icon _playbackTrayIcon;

        public frmDefault()
        {
            InitializeComponent();

            LoadSettings();

            // Como o form nasce oculto, o handle só iria existir na primeira exibição. Sem ele, o primeiro clique no ícone seria gasto criando a janela em vez de abrir o menu
            _ = Handle;
        }

        protected override void SetVisibleCore(bool value)
        {
            base.SetVisibleCore(_allowVisible && value);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _muteOnLockMonitor.Dispose();
            _hotkeys.Dispose();
            _trayHeaderFont?.Dispose();
            RestoreDefaultTrayIcon();
            _playbackTrayIcon?.Dispose();

            base.OnFormClosed(e);
        }

        private void LoadSettings()
        {
            _settings = AppConfig.Load();

            #region Áudio

            cbAudioMuteOnLock.Checked = _settings.Audio.MuteOnLock;
            _muteOnLockMonitor.SetEnabled(cbAudioMuteOnLock.Checked);
            cbAudioMuteOnLock.CheckedChanged += cbMuteOnLock_CheckedChanged;

            cbAudioDeviceChangeNotification.Checked = _settings.Audio.ShowNotificationOnDeviceChange;
            cbAudioDeviceChangeNotification.CheckedChanged += cbAudioDeviceChangeNotification_CheckedChanged;

            AudioDeviceCatalog.Refresh(_settings.Audio.Devices);
            AppConfig.Save(_settings);

            lvAudioDeviceList(lvAudioPlayback, _settings.Audio.Devices.Playback);
            lvAudioDeviceList(lvAudioRecord, _settings.Audio.Devices.Recording);

            lvAudioPlayback.ItemChecked += lvAudioPlayback_ItemChecked;
            lvAudioRecord.ItemChecked += lvAudioRecord_ItemChecked;

            txtDevicePlaybackShortcut.Text = _settings.Audio.Devices.PlaybackShortcut;
            txtDeviceRecordShortcut.Text = _settings.Audio.Devices.RecordingShortcut;

            RefreshHotkeys();
            ApplyPlaybackTrayIcon();

            #endregion

            #region Geral

            cbGeralInitializeWindows.Checked = _settings.General.AutoStart;

            try
            {
                WindowsAutoStart.SetEnabled(cbGeralInitializeWindows.Checked);
            }
            catch
            {
            }

            cbGeralInitializeWindows.CheckedChanged += cbGeralInitializeWindows_CheckedChanged;

            #endregion

            #region Painel de Controle e Personalização (Windows Actions)

            foreach ((CheckBox box, IWindowsAction action) in WindowsActions())
                ActionBinding.Load(box, action);

            foreach ((CheckBox box, IWindowsAction action) in WindowsActions())
                ActionBinding.Bind(box, action);

            #endregion
        }

        private void frmDefault_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;

                _allowVisible = false;

                ShowInTaskbar = false;

                Hide();
            }
        }

        private (CheckBox Box, IWindowsAction Action)[] WindowsActions()
        {
            (CheckBox Box, IWindowsAction Action)[] windowsActions =
            [
                (cbPainelControleUAC, Actions.PainelControle.DisableUac),
                (cbPainelControleNoSoundScheme, Actions.PainelControle.NoSoundScheme),
                (cbPainelControleDisableStartupSound, Actions.PainelControle.DisableStartupSound),
                (cbPainelControleLidCloseDoNothing, Actions.PainelControle.LidCloseDoNothing),
                (cbPainelControleNeverSleepOrTurnOffDisplay, Actions.PainelControle.NeverSleepOrTurnOffDisplay),
                (cbPersonalizacaoTaskbarAlignAndSettings, Actions.Personalizacao.TaskbarAlignAndSettings),
                (cbPersonalizacaoDisableItemsTaskbar, Actions.Personalizacao.DisableItemsTaskbar)
            ];

            return windowsActions;
        }

        private void cbMuteOnLock_CheckedChanged(object sender, EventArgs e)
        {
            _settings.Audio.MuteOnLock = cbAudioMuteOnLock.Checked;

            AppConfig.Save(_settings);

            _muteOnLockMonitor.SetEnabled(cbAudioMuteOnLock.Checked);
        }

        private void cbAudioDeviceChangeNotification_CheckedChanged(object sender, EventArgs e)
        {
            _settings.Audio.ShowNotificationOnDeviceChange = cbAudioDeviceChangeNotification.Checked;

            AppConfig.Save(_settings);
        }

        private void cbGeralInitializeWindows_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                WindowsAutoStart.SetEnabled(cbGeralInitializeWindows.Checked);
            }
            catch (Exception ex)
            {
                cbGeralInitializeWindows.CheckedChanged -= cbGeralInitializeWindows_CheckedChanged;
                cbGeralInitializeWindows.Checked = !cbGeralInitializeWindows.Checked;
                cbGeralInitializeWindows.CheckedChanged += cbGeralInitializeWindows_CheckedChanged;

                MessageBox.Show(
                    ex.Message,
                    "Windows Configurations",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            _settings.General.AutoStart = cbGeralInitializeWindows.Checked;

            AppConfig.Save(_settings);
        }

        private static void lvAudioDeviceList(ListView list, List<AudioDeviceEntry> devices)
        {
            list.BeginUpdate();
            list.Items.Clear();

            if (list.View != View.Details)
                list.View = View.Details;

            list.HeaderStyle = ColumnHeaderStyle.None;
            list.FullRowSelect = true;

            if (list.Columns.Count == 0)
                list.Columns.Add(string.Empty, list.ClientSize.Width - 4);

            ImageList previousIcons = list.SmallImageList;
            int size = 32 * list.DeviceDpi / 96;

            ImageList icons = new()
            {
                ColorDepth = ColorDepth.Depth32Bit,
                ImageSize = new Size(size, size)
            };

            list.SmallImageList = icons;

            foreach (AudioDeviceEntry device in devices)
            {
                list.Items.Add(new ListViewItem(device.Name)
                {
                    Tag = device.Id,
                    Checked = device.Enabled,
                    ImageIndex = AddDeviceIcon(icons, device.IconPath, size)
                });
            }

            list.EndUpdate();

            previousIcons?.Dispose();
        }

        private static int AddDeviceIcon(ImageList icons, string iconPath, int size)
        {
            using Icon icon = AudioDeviceIcon.Load(iconPath, size);

            if (icon is null)
                return -1;

            icons.Images.Add(icon);

            return icons.Images.Count - 1;
        }

        private void lvAudioPlayback_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            lvAudioDeviceEnable(_settings.Audio.Devices.Playback, e.Item);
        }

        private void lvAudioRecord_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            lvAudioDeviceEnable(_settings.Audio.Devices.Recording, e.Item);
        }

        private void lvAudioDeviceEnable(List<AudioDeviceEntry> devices, ListViewItem item)
        {
            string id = item.Tag as string;

            if (string.IsNullOrEmpty(id))
                return;

            AudioDeviceEntry device = devices.Find(entry => entry.Id == id);

            if (device is null || device.Enabled == item.Checked)
                return;

            device.Enabled = item.Checked;
            AppConfig.Save(_settings);
        }

        private void txtDevicePlaybackShortcut_KeyDown(object sender, KeyEventArgs e)
        {
            CaptureShortcut(txtDevicePlaybackShortcut, e, isPlayback: true);
        }

        private void txtDeviceRecordShortcut_KeyDown(object sender, KeyEventArgs e)
        {
            CaptureShortcut(txtDeviceRecordShortcut, e, isPlayback: false);
        }

        private void CaptureShortcut(TextBox box, KeyEventArgs e, bool isPlayback)
        {
            // Impede que a tecla seja digitada: o campo mostra apenas a combinação formatada.
            e.SuppressKeyPress = true;
            e.Handled = true;

            if (e.KeyCode is Keys.Escape or Keys.Back or Keys.Delete)
            {
                SetShortcut(box, string.Empty, isPlayback);
                return;
            }

            string shortcut = ShortcutKeys.Format(e.KeyData);

            box.Text = shortcut;
            box.SelectionStart = box.TextLength;

            if (ShortcutKeys.IsComplete(e.KeyData))
                SetShortcut(box, shortcut, isPlayback);
        }

        private void SetShortcut(TextBox box, string shortcut, bool isPlayback)
        {
            box.Text = shortcut;
            box.SelectionStart = box.TextLength;

            if (isPlayback)
                _settings.Audio.Devices.PlaybackShortcut = shortcut;
            else
                _settings.Audio.Devices.RecordingShortcut = shortcut;

            AppConfig.Save(_settings);
        }

        /// <summary>
        /// Com o atalho registrado, a combinação não chega ao campo: os atalhos ficam
        /// suspensos enquanto o usuário está digitando neles.
        /// </summary>
        private void shortcutInput_Enter(object sender, EventArgs e)
        {
            _hotkeys.Clear();
        }

        private void shortcutInput_Leave(object sender, EventArgs e)
        {
            RefreshHotkeys();
        }

        private void RefreshHotkeys()
        {
            _hotkeys.Clear();

            if (ShortcutKeys.TryParse(_settings.Audio.Devices.PlaybackShortcut, out Keys playback))
                _hotkeys.Register(playback, () => CycleDefaultDevice(isPlayback: true));

            if (ShortcutKeys.TryParse(_settings.Audio.Devices.RecordingShortcut, out Keys recording))
                _hotkeys.Register(recording, () => CycleDefaultDevice(isPlayback: false));
        }

        private void CycleDefaultDevice(bool isPlayback)
        {
            List<AudioDeviceEntry> devices = isPlayback
                ? _settings.Audio.Devices.Playback.FindAll(device => device.Enabled)
                : _settings.Audio.Devices.Recording.FindAll(device => device.Enabled);

            if (devices.Count == 0)
                return;

            string currentId = isPlayback
                ? AudioEndpointEnumerator.GetDefaultPlaybackId()
                : AudioEndpointEnumerator.GetDefaultRecordingId();

            int current = devices.FindIndex(device => string.Equals(device.Id, currentId, StringComparison.OrdinalIgnoreCase));
            AudioDeviceEntry next = devices[current < 0 ? 0 : (current + 1) % devices.Count];

            SetTrayDefaultDevice(next.Id, isPlayback);
        }

        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                RebuildDeviceTrayMenu();
                ShowTrayMenu(cmDevices);

                return;
            }

            if (e.Button == MouseButtons.Right || e.Button == MouseButtons.Middle)
                ShowTrayMenu(cmOptions);
        }

        private void configuraçõesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _allowVisible = true;

            ShowInTaskbar = true;

            Show();

            Activate();
        }

        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notifyIcon.Visible = false;

            Application.Exit();
        }

        private void RebuildDeviceTrayMenu()
        {
            _trayHeaderFont ??= new Font(cmDevices.Font, FontStyle.Bold);

            cmDevices.Items.Clear();

            List<AudioDeviceEntry> playback = _settings.Audio.Devices.Playback.FindAll(device => device.Enabled);
            List<AudioDeviceEntry> recording = _settings.Audio.Devices.Recording.FindAll(device => device.Enabled);

            string playbackDefault = AudioEndpointEnumerator.GetDefaultPlaybackId();
            string recordingDefault = AudioEndpointEnumerator.GetDefaultRecordingId();

            AddDeviceTraySection("Reprodução", playback, playbackDefault, isPlayback: true);

            if (playback.Count > 0 && recording.Count > 0)
                cmDevices.Items.Add(new ToolStripSeparator());

            AddDeviceTraySection("Gravação", recording, recordingDefault, isPlayback: false);

            if (cmDevices.Items.Count == 0)
            {
                cmDevices.Items.Add(new ToolStripMenuItem("Nenhum dispositivo selecionado")
                {
                    Enabled = false
                });
            }
        }

        private void AddDeviceTraySection(string title, List<AudioDeviceEntry> devices, string defaultId, bool isPlayback)
        {
            if (devices.Count == 0)
                return;

            cmDevices.Items.Add(new ToolStripLabel(title)
            {
                Font = _trayHeaderFont
            });

            foreach (AudioDeviceEntry device in devices)
            {
                ToolStripMenuItem item = new(device.Name)
                {
                    Tag = device.Id,
                    Checked = string.Equals(device.Id, defaultId, StringComparison.OrdinalIgnoreCase)
                };

                string id = device.Id;
                item.Click += (_, _) => SetTrayDefaultDevice(id, isPlayback);
                cmDevices.Items.Add(item);
            }
        }

        private bool SetTrayDefaultDevice(string deviceId, bool isPlayback)
        {
            if (!AudioDefaultDevice.SetDefault(deviceId))
                return false;

            if (isPlayback)
                _settings.Audio.Devices.PlaybackDefault = deviceId;
            else
                _settings.Audio.Devices.RecordingDefault = deviceId;

            AppConfig.Save(_settings);

            if (isPlayback)
                ApplyPlaybackTrayIcon();

            ShowDeviceChangeNotification(deviceId, isPlayback);

            return true;
        }

        private void ShowDeviceChangeNotification(string deviceId, bool isPlayback)
        {
            if (!_settings.Audio.ShowNotificationOnDeviceChange)
                return;

            List<AudioDeviceEntry> devices = isPlayback ? _settings.Audio.Devices.Playback : _settings.Audio.Devices.Recording;

            AudioDeviceEntry device = devices.Find(entry => string.Equals(entry.Id, deviceId, StringComparison.OrdinalIgnoreCase));

            string name = string.IsNullOrWhiteSpace(device?.Name) ? deviceId : device.Name;

            notifyIcon.ShowBalloonTip(1000, isPlayback ? "Reprodução" : "Gravação", name, ToolTipIcon.Info);
        }

        private void ApplyPlaybackTrayIcon()
        {
            _defaultTrayIcon ??= notifyIcon.Icon;

            string deviceId = _settings.Audio.Devices.PlaybackDefault;

            if (string.IsNullOrEmpty(deviceId))
                deviceId = AudioEndpointEnumerator.GetDefaultPlaybackId();

            AudioDeviceEntry device = _settings.Audio.Devices.Playback.Find(entry =>
                string.Equals(entry.Id, deviceId, StringComparison.OrdinalIgnoreCase));

            Icon loaded = AudioDeviceIcon.Load(device?.IconPath, SystemInformation.SmallIconSize.Width);
            Icon previous = _playbackTrayIcon;

            notifyIcon.Icon = loaded ?? _defaultTrayIcon;
            _playbackTrayIcon = loaded;

            if (!string.IsNullOrWhiteSpace(device?.Name))
                notifyIcon.Text = device.Name.Length <= 63 ? device.Name : device.Name[..63];
            else
                notifyIcon.Text = "Windows Configurations";

            previous?.Dispose();
        }

        private void RestoreDefaultTrayIcon()
        {
            if (_defaultTrayIcon is not null)
                notifyIcon.Icon = _defaultTrayIcon;
        }

        private void ShowTrayMenu(ContextMenuStrip menu)
        {
            SetForegroundWindow(Handle);

            menu.Show(Cursor.Position, ToolStripDropDownDirection.AboveLeft);

            PostMessage(Handle, WM_NULL, IntPtr.Zero, IntPtr.Zero);
        }

        private const uint WM_NULL = 0;

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
    }
}
