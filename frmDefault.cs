using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using SoundSwitch.Configuration;
using SoundSwitch.Configuration.Audio;
using SoundSwitch.Features.Audio;
using SoundSwitch.Features.Shortcuts;
using SoundSwitch.Features.Startup;
using SoundSwitch.Updater;

namespace SoundSwitch
{
    public partial class frmDefault : Form
    {
        private readonly MuteOnLockMonitor _muteOnLockMonitor = new();
        private readonly HotkeyManager _hotkeys = new();
        private readonly AudioDeviceWatcher _deviceWatcher = new();
        private readonly System.Windows.Forms.Timer _deviceRefreshDelay = new() { Interval = 500 };
        private AppConfiguration _settings;
        private bool _allowVisible;
        private Font _trayHeaderFont;
        private Icon _defaultTrayIcon;
        private AudioDeviceIcon _playbackTrayIcon;
        private AvailableUpdate _availableUpdate;
        private bool _updateBalloon;
        private bool _updatingDeviceLists;

        public frmDefault()
        {
            InitializeComponent();

            LoadSettings();
            InitTrayOptionsMenu();

            // Como o form nasce oculto, o handle só iria existir na primeira exibição. Sem ele, o primeiro clique no ícone seria gasto criando a janela em vez de abrir o menu
            _ = Handle;

            StartDeviceWatcher();

            BeginInvoke(CheckForUpdateOnStart);
        }

        protected override void SetVisibleCore(bool value)
        {
            base.SetVisibleCore(_allowVisible && value);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _muteOnLockMonitor.Dispose();
            _hotkeys.Dispose();
            _deviceWatcher.Dispose();
            _deviceRefreshDelay.Dispose();
            _trayHeaderFont?.Dispose();

            if (_defaultTrayIcon is not null)
                notifyIcon.Icon = _defaultTrayIcon;

            _playbackTrayIcon?.Dispose();

            base.OnFormClosed(e);
        }

        private void LoadSettings()
        {
            _settings = AppConfig.Load();

            AudioDeviceCatalog.Refresh(_settings.Audio.Devices);
            ApplyPreferredDevices();
            AppConfig.Save(_settings);

            #region Reprodução

            FillAudioDeviceList(lvAudioReproducao, _settings.Audio.Devices.Playback);
            lvAudioReproducao.ItemChecked += lvAudioReproducao_ItemChecked;
            txtReproducaoAtalho.Text = _settings.Audio.Devices.PlaybackShortcut;

            #endregion

            #region Gravação

            FillAudioDeviceList(lvAudioGravacao, _settings.Audio.Devices.Recording);
            lvAudioGravacao.ItemChecked += lvAudioGravacao_ItemChecked;
            txtGravacaoAtalho.Text = _settings.Audio.Devices.RecordingShortcut;

            #endregion

            RefreshHotkeys();
            ApplyPlaybackTrayIcon();

            #region Configurações

            cbConfigGeralIniciarWindows.Checked = _settings.General.AutoStart;

            try
            {
                WindowsAutoStart.SetEnabled(cbConfigGeralIniciarWindows.Checked);
            }
            catch
            {
            }

            cbConfigGeralIniciarWindows.CheckedChanged += cbConfigGeralIniciarWindows_CheckedChanged;

            cbConfigAudioMudoBloquear.Checked = _settings.Audio.MuteOnLock;
            _muteOnLockMonitor.SetEnabled(cbConfigAudioMudoBloquear.Checked);
            cbConfigAudioMudoBloquear.CheckedChanged += cbConfigAudioMudoBloquear_CheckedChanged;

            cbConfigNotificacoesMostrar.Checked = _settings.Audio.ShowNotificationOnDeviceChange;
            cbConfigNotificacoesMostrar.CheckedChanged += cbConfigNotificacoesMostrar_CheckedChanged;

            lbConfigVersaoAtual.Text = $"Atual: {AppVersion.CurrentDisplay}";
            RefreshConfigVersaoDisponivel();

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

        private void cbConfigAudioMudoBloquear_CheckedChanged(object sender, EventArgs e)
        {
            _settings.Audio.MuteOnLock = cbConfigAudioMudoBloquear.Checked;

            AppConfig.Save(_settings);

            _muteOnLockMonitor.SetEnabled(cbConfigAudioMudoBloquear.Checked);
        }

        private void cbConfigNotificacoesMostrar_CheckedChanged(object sender, EventArgs e)
        {
            _settings.Audio.ShowNotificationOnDeviceChange = cbConfigNotificacoesMostrar.Checked;

            AppConfig.Save(_settings);
        }

        private void cbConfigGeralIniciarWindows_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                WindowsAutoStart.SetEnabled(cbConfigGeralIniciarWindows.Checked);
            }
            catch (Exception ex)
            {
                cbConfigGeralIniciarWindows.CheckedChanged -= cbConfigGeralIniciarWindows_CheckedChanged;
                cbConfigGeralIniciarWindows.Checked = !cbConfigGeralIniciarWindows.Checked;
                cbConfigGeralIniciarWindows.CheckedChanged += cbConfigGeralIniciarWindows_CheckedChanged;

                MessageBox.Show(
                    ex.Message,
                    "SoundSwitch",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            _settings.General.AutoStart = cbConfigGeralIniciarWindows.Checked;

            AppConfig.Save(_settings);
        }

        private void StartDeviceWatcher()
        {
            _deviceRefreshDelay.Tick += deviceRefreshDelay_Tick;
            _deviceWatcher.Changed += OnAudioDevicesChanged;
            _deviceWatcher.Start();
        }

        private void OnAudioDevicesChanged()
        {
            if (IsDisposed || !IsHandleCreated)
                return;

            try
            {
                // A notificação vem de uma thread do COM e costuma chegar em rajada: o timer
                // devolve o trabalho para a thread da interface e agrupa os eventos em uma leitura só.
                BeginInvoke(() =>
                {
                    _deviceRefreshDelay.Stop();
                    _deviceRefreshDelay.Start();
                });
            }
            catch (Exception ex) when (ex is InvalidOperationException or ObjectDisposedException)
            {
                // O form foi encerrado entre a checagem e o BeginInvoke.
            }
        }

        private void deviceRefreshDelay_Tick(object sender, EventArgs e)
        {
            _deviceRefreshDelay.Stop();

            RefreshAudioDevices();
        }

        private void RefreshAudioDevices()
        {
            AudioDeviceCatalog.Refresh(_settings.Audio.Devices);
            ApplyPreferredDevices();

            FillAudioDeviceList(lvAudioReproducao, _settings.Audio.Devices.Playback);
            FillAudioDeviceList(lvAudioGravacao, _settings.Audio.Devices.Recording);

            ApplyPlaybackTrayIcon();
        }

        /// <summary>
        /// Preferido conectado: volta para ele. Preferido desconectado: o próximo marcado
        /// e conectado na lista. A troca automática não altera PlaybackDefault/RecordingDefault.
        /// </summary>
        private void ApplyPreferredDevices()
        {
            ApplyPreferredDevice(isPlayback: true);
            ApplyPreferredDevice(isPlayback: false);
        }

        private void ApplyPreferredDevice(bool isPlayback)
        {
            AudioDevicesSettings devices = _settings.Audio.Devices;
            string preferredId = devices.PreferredId(isPlayback);

            if (string.IsNullOrEmpty(preferredId))
                return;

            string currentId = AudioEndpointEnumerator.GetDefaultId(isPlayback);
            AudioDeviceEntry preferred = devices.FindById(isPlayback, preferredId);

            if (preferred is { Enabled: true, Connected: true })
            {
                if (!string.Equals(currentId, preferred.Id, StringComparison.OrdinalIgnoreCase))
                    SetWindowsDefault(preferred.Id, isPlayback, updatePreferred: false);

                return;
            }

            if (preferred is null || !preferred.Enabled)
                return;

            string fallbackId = NextEnabledConnected(devices.Devices(isPlayback), preferredId);

            if (string.IsNullOrEmpty(fallbackId)
                || string.Equals(currentId, fallbackId, StringComparison.OrdinalIgnoreCase))
                return;

            SetWindowsDefault(fallbackId, isPlayback, updatePreferred: false);
        }

        /// <summary>
        /// Mesma ordem da lista marcada: o próximo habilitado e conectado depois do âncora.
        /// </summary>
        private static string NextEnabledConnected(List<AudioDeviceEntry> devices, string afterId)
        {
            List<AudioDeviceEntry> enabled = devices.FindAll(device => device.Enabled);

            int index = enabled.FindIndex(device =>
                string.Equals(device.Id, afterId, StringComparison.OrdinalIgnoreCase));

            if (index < 0 || enabled.Count < 2)
                return null;

            for (int offset = 1; offset < enabled.Count; offset++)
            {
                AudioDeviceEntry candidate = enabled[(index + offset) % enabled.Count];

                if (candidate.Connected)
                    return candidate.Id;
            }

            return null;
        }

        private void FillAudioDeviceList(ListView list, List<AudioDeviceEntry> devices)
        {
            _updatingDeviceLists = true;
            list.BeginUpdate();
            ImageList previousIcons = list.SmallImageList;

            try
            {
                list.Items.Clear();

                if (list.Columns.Count == 0)
                {
                    list.View = View.Details;
                    list.HeaderStyle = ColumnHeaderStyle.None;
                    list.FullRowSelect = true;
                    list.Columns.Add(string.Empty, list.ClientSize.Width - 4);
                }

                int size = 32 * list.DeviceDpi / 96;

                ImageList icons = new()
                {
                    ColorDepth = ColorDepth.Depth32Bit,
                    ImageSize = new Size(size, size)
                };

                list.SmallImageList = icons;

                foreach (AudioDeviceEntry device in devices)
                {
                    if (!device.Connected)
                        continue;

                    list.Items.Add(new ListViewItem(device.Name)
                    {
                        Tag = device.Id,
                        Checked = device.Enabled,
                        ImageIndex = AddDeviceIcon(icons, device.IconPath, size)
                    });
                }
            }
            finally
            {
                list.EndUpdate();
                _updatingDeviceLists = false;
                previousIcons?.Dispose();
            }
        }

        private static int AddDeviceIcon(ImageList icons, string iconPath, int size)
        {
            using AudioDeviceIcon icon = AudioDeviceIcon.Load(iconPath, size);

            if (icon is null)
                return -1;

            icons.Images.Add(icon.Icon);

            return icons.Images.Count - 1;
        }

        private void lvAudioReproducao_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            EnableAudioDevice(_settings.Audio.Devices.Playback, e.Item);
        }

        private void lvAudioGravacao_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            EnableAudioDevice(_settings.Audio.Devices.Recording, e.Item);
        }

        private void EnableAudioDevice(List<AudioDeviceEntry> devices, ListViewItem item)
        {
            if (_updatingDeviceLists)
                return;

            string id = item.Tag as string;

            if (string.IsNullOrEmpty(id))
                return;

            AudioDeviceEntry device = devices.Find(entry => entry.Id == id);

            if (device is null || device.Enabled == item.Checked)
                return;

            device.Enabled = item.Checked;
            AppConfig.Save(_settings);
        }

        private void txtReproducaoAtalho_KeyDown(object sender, KeyEventArgs e)
        {
            CaptureShortcut(txtReproducaoAtalho, e, isPlayback: true);
        }

        private void txtGravacaoAtalho_KeyDown(object sender, KeyEventArgs e)
        {
            CaptureShortcut(txtGravacaoAtalho, e, isPlayback: false);
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

            _settings.Audio.Devices.SetShortcut(isPlayback, shortcut);

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

            if (ShortcutKeys.TryParse(_settings.Audio.Devices.Shortcut(true), out Keys playback))
                _hotkeys.Register(playback, () => CycleDefaultDevice(isPlayback: true));

            if (ShortcutKeys.TryParse(_settings.Audio.Devices.Shortcut(false), out Keys recording))
                _hotkeys.Register(recording, () => CycleDefaultDevice(isPlayback: false));
        }

        private void CycleDefaultDevice(bool isPlayback)
        {
            AudioDeviceCatalog.Refresh(_settings.Audio.Devices);

            List<AudioDeviceEntry> devices = _settings.Audio.Devices.EnabledConnected(isPlayback);

            if (devices.Count == 0)
                return;

            string currentId = AudioEndpointEnumerator.GetDefaultId(isPlayback);

            int current = devices.FindIndex(device => string.Equals(device.Id, currentId, StringComparison.OrdinalIgnoreCase));
            AudioDeviceEntry next = devices[current < 0 ? 0 : (current + 1) % devices.Count];

            SetWindowsDefault(next.Id, isPlayback, updatePreferred: true);
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

        private void InitTrayOptionsMenu()
        {
            _trayHeaderFont ??= new Font(cmOptions.Font, FontStyle.Bold);

            lblTrayAppVersion.Font = _trayHeaderFont;
            lblTrayAppVersion.Text = $"{Application.ProductName} ({AppVersion.CurrentDisplay})";

            notifyIcon.BalloonTipClicked += notifyIcon_BalloonTipClicked;
        }

        private async void CheckForUpdateOnStart()
        {
            try
            {
                AvailableUpdate update = await UpdateChecker.CheckAsync();

                if (update is null || IsDisposed)
                    return;

                ApplyAvailableUpdate(update);
            }
            catch
            {
            }
        }

        private void RefreshConfigVersaoDisponivel()
        {
            string version = _availableUpdate?.VersionDisplay;
            bool available = !string.IsNullOrWhiteSpace(version);

            lbConfigVersaoDisponivel.Text = available
                ? $"Disponível: {AppVersion.Format(version)}"
                : "Disponível: —";

            btnConfigVersaoAtualizar.Visible = available;
            pbConfigVersao.Visible = available;

            if (!available)
            {
                pbConfigVersao.Style = ProgressBarStyle.Blocks;
                pbConfigVersao.Value = 0;
            }
        }

        private void ApplyAvailableUpdate(AvailableUpdate update)
        {
            _availableUpdate = update;
            RefreshConfigVersaoDisponivel();

            _updateBalloon = true;
            notifyIcon.ShowBalloonTip(
                1000,
                "SoundSwitch",
                $"Atualização para SoundSwitch ({update.VersionDisplay}) está disponível",
                ToolTipIcon.Info);
        }

        private void notifyIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            if (_updateBalloon)
                ShowSettings(tabConfiguracoes);
        }

        private async void btnConfigVersaoAtualizar_Click(object sender, EventArgs e)
        {
            if (_availableUpdate is null)
                return;

            btnConfigVersaoAtualizar.Enabled = false;

            pbConfigVersao.Value = 0;
            pbConfigVersao.Style = ProgressBarStyle.Marquee;
            pbConfigVersao.Visible = true;

            Progress<int> progress = new(percent =>
            {
                pbConfigVersao.Style = ProgressBarStyle.Blocks;
                pbConfigVersao.Value = Math.Clamp(percent, 0, 100);
            });

            try
            {
                string installer = await UpdateInstaller.DownloadAsync(_availableUpdate, progress);

                UpdateInstaller.Start(installer);

                notifyIcon.Visible = false;
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "SoundSwitch",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                pbConfigVersao.Style = ProgressBarStyle.Blocks;
                pbConfigVersao.Value = 0;
                btnConfigVersaoAtualizar.Enabled = true;
            }
        }

        private void ShowSettings(TabPage tab = null)
        {
            _allowVisible = true;

            ShowInTaskbar = true;

            RefreshAudioDevices();

            if (tab is not null)
                tabDefault.SelectedTab = tab;

            Show();

            Activate();
        }

        private void configuraçõesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSettings();
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

            AudioDeviceCatalog.Refresh(_settings.Audio.Devices);

            ApplyPlaybackTrayIcon();

            List<AudioDeviceEntry> playback = _settings.Audio.Devices.EnabledConnected(true);
            List<AudioDeviceEntry> recording = _settings.Audio.Devices.EnabledConnected(false);

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
                item.Click += (_, _) => SetWindowsDefault(id, isPlayback, updatePreferred: true);
                cmDevices.Items.Add(item);
            }
        }

        private bool SetWindowsDefault(string deviceId, bool isPlayback, bool updatePreferred)
        {
            if (!AudioDefaultDevice.SetDefault(deviceId))
                return false;

            if (updatePreferred)
            {
                _settings.Audio.Devices.SetPreferredId(isPlayback, deviceId);
                AppConfig.Save(_settings);
            }

            if (isPlayback)
                ApplyPlaybackTrayIcon();

            ShowDeviceChangeNotification(deviceId, isPlayback);

            return true;
        }

        private void ShowDeviceChangeNotification(string deviceId, bool isPlayback)
        {
            if (!_settings.Audio.ShowNotificationOnDeviceChange)
                return;

            AudioDeviceEntry device = _settings.Audio.Devices.FindById(isPlayback, deviceId);

            string name = string.IsNullOrWhiteSpace(device?.Name) ? deviceId : device.Name;

            _updateBalloon = false;
            notifyIcon.ShowBalloonTip(1000, isPlayback ? "Reprodução" : "Gravação", name, ToolTipIcon.Info);
        }

        private void ApplyPlaybackTrayIcon()
        {
            _defaultTrayIcon ??= notifyIcon.Icon;

            // O padrão real do Windows vem primeiro: assim o ícone acompanha também as trocas
            // feitas fora do app, como a que o próprio sistema faz ao desconectar um fone.
            string deviceId = AudioEndpointEnumerator.GetDefaultPlaybackId();

            if (string.IsNullOrEmpty(deviceId))
                deviceId = _settings.Audio.Devices.PlaybackDefault;

            AudioDeviceEntry device = _settings.Audio.Devices.FindById(true, deviceId);

            AudioDeviceIcon loaded = AudioDeviceIcon.Load(device?.IconPath, SystemInformation.SmallIconSize.Width);
            AudioDeviceIcon previous = _playbackTrayIcon;

            notifyIcon.Icon = loaded?.Icon ?? _defaultTrayIcon;
            _playbackTrayIcon = loaded;

            if (!string.IsNullOrWhiteSpace(device?.Name))
                notifyIcon.Text = device.Name.Length <= 63 ? device.Name : device.Name[..63];
            else
                notifyIcon.Text = "SoundSwitch";

            previous?.Dispose();
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
